using System;
using System.Linq;
using System.Threading.Tasks;
using CommandDotNet;
using CommandDotNet.Rendering;
using Grpc.Core;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Caching;
using Polly.Contrib.WaitAndRetry;
using Polly.Registry;
using Polly.Timeout;

namespace ResilienceDemo.Battery
{
    public static class PolicyContainerExtensions
    {
        private const int MaxRetries = 5;

        public static IServiceCollection AddDefaultPolicies(this IServiceCollection serviceCollection)
        {
            serviceCollection
                .AddSingleton<IReadOnlyPolicyRegistry<string>>(provider =>
                {
                    return GetDefaultRegistry(provider);
                });
            return serviceCollection;
        }

        private static PolicyRegistry GetDefaultRegistry(IServiceProvider provider)
        {
            var console = provider.GetService<IConsole>();
            var policyRegistry = new PolicyRegistry
            {
                {
                    RetryPolicyKey.NoRetry.ToString(),
                    Policy.NoOpAsync()
                },
                {
                    RetryPolicyKey.BasicRetryOnRpc.ToString(),
                    Policy
                        .Handle<RpcException>()
                        .Or<TimeoutRejectedException>()
                        .RetryAsync(MaxRetries, (exception, retryAttempt, context) =>
                        {
                            console.Out.WriteLine($"Operation: {context.OperationKey}; Attempt {retryAttempt - 1} failed: {exception.Message}. Retrying.");
                            return Task.CompletedTask;
                        })
                },
                {
                    RetryPolicyKey.RetryOnRpcWithExponentialBackoff.ToString(),
                    Policy
                        .Handle<RpcException>()
                        .WaitAndRetryAsync(Backoff.ExponentialBackoff(
                            TimeSpan.FromSeconds(1),
                            MaxRetries), (exception, timeSpan, retryAttempt, context) =>
                        {
                            console.Out.WriteLine(
                                $"Operation: {context.OperationKey}; TimeSpan: {timeSpan.ToString()}. Attempt {retryAttempt - 1} failed: {exception.Message}. Retrying.");
                            return Task.CompletedTask;
                        })
                },
                {
                    RetryPolicyKey.RetryOnRpcWithJitter.ToString(),
                    Policy
                        .Handle<RpcException>()
                        .WaitAndRetryAsync(MaxRetries,
                            retryAttempt =>
                            {
                                var backoffSpans =
                                    Backoff
                                        .DecorrelatedJitterBackoffV2(
                                            TimeSpan.FromSeconds(1),
                                            MaxRetries)
                                        .ToList();
                                return backoffSpans[retryAttempt - 1];
                            },
                            (exception, timeSpan, retryAttempt, context) =>
                            {
                                console.Out.WriteLine(
                                    $"Operation: {context.OperationKey}; TimeSpan: {timeSpan.ToString()}. Attempt {retryAttempt - 1} failed: {exception.Message}. Retrying.");
                                return Task.CompletedTask;
                            })
                },
                {
                    CachePolicyKey.InMemoryCache.ToString(),
                    Policy
                        .CacheAsync(
                        provider.GetService<IAsyncCacheProvider>(),
                        TimeSpan.FromMinutes(5),
                        (policyContext, cacheKey) => console.WriteLine($"Operation {policyContext.OperationKey}: Cache get {cacheKey}"),
                        (policyContext, cacheKey) => console.WriteLine($"Operation {policyContext.OperationKey}: Cache miss {cacheKey}"),
                        (policyContext, cacheKey) => console.WriteLine($"Operation {policyContext.OperationKey}: Cache put {cacheKey}"),
                        (policyContext, cacheKey, exception) => console.WriteLine($"Operation {policyContext.OperationKey}: Cache get error {cacheKey}; {exception}"),
                        (policyContext, cacheKey, exception) => console.WriteLine($"Operation {policyContext.OperationKey}: Cache put error {cacheKey}; {exception}"))
                },
                {
                    CachePolicyKey.NoCache.ToString(),
                    Policy.NoOpAsync()
                },
                {
                    TimeoutPolicyKey.NoTimeout.ToString(),
                    Policy.NoOpAsync()
                },
                {
                    TimeoutPolicyKey.DefaultPessimisticTimeout.ToString(),
                    Policy.TimeoutAsync(TimeSpan.FromMilliseconds(500), TimeoutStrategy.Pessimistic, (context, span, task) =>
                    {
                        // do not await, otherwise policy is useless.
                        task.ContinueWith(t =>
                        {
                            // ContinueWith important the abandoned task may still be executing, when the caller times out 

                            if (t.IsFaulted)
                            {
                                console.Out.WriteLine(
                                    $"Operation {context.OperationKey}: execution timed out after {span.TotalSeconds} seconds, eventually terminated with: {t.Exception.Message}.");
                            }
                            else if (t.IsCanceled)
                            {
                                // (If the executed delegates do not honour cancellation, this IsCanceled branch may never be hit.
                                // It can be good practice however to include, in case a Policy configured with TimeoutStrategy.Pessimistic
                                // is used to execute a delegate honouring cancellation.)  
                                console.Out.WriteLine(
                                    $"Operation {context.OperationKey}: execution timed out after {span.TotalSeconds} seconds, task cancelled.");
                            }
                            else
                            {
                                // extra logic (if desired) for tasks which complete, despite the caller having 'walked away' earlier due to timeout.
                                console.Out.WriteLine(
                                    $"Operation {context.OperationKey}: execution timed out after {span.TotalSeconds} seconds, task completed.");
                            }

                            // Additionally, clean up any resources ...
                        });
                        
                        console.Out.WriteLine($"Operation {context.OperationKey} timed out.");
                        return Task.CompletedTask;
                    })
                },
                {
                    TimeoutPolicyKey.DefaultOptimisticTimeout.ToString(),
                    Policy.TimeoutAsync(TimeSpan.FromMilliseconds(500), TimeoutStrategy.Optimistic,
                        (context, span, abandonedTask) =>
                        {
                            console.Out.WriteLine($"Operation: {context.OperationKey}, timeout after {span}. ");
                            abandonedTask.ContinueWith(t =>
                            {
                                if (t.IsFaulted)
                                {
                                    console.Out.WriteLine(
                                        $"Operation {context.OperationKey}: execution timed out after {span.TotalSeconds} seconds, eventually terminated with: {t.Exception.Message}.");
                                }
                                else if (t.IsCanceled)
                                {
                                    console.Out.WriteLine(
                                        $"Operation {context.OperationKey}: execution timed out after {span.TotalSeconds} seconds, task cancelled.");
                                }
                                else
                                {
                                    console.Out.WriteLine(
                                        $"Operation {context.OperationKey}: execution timed out after {span.TotalSeconds} seconds, task completed.");
                                }
                            });
                            return Task.CompletedTask;
                        })
                },
                {
                    CircuitBreakerPolicyKey.NoBreaker.ToString(),
                    Policy.NoOpAsync()
                },
                {
                    CircuitBreakerPolicyKey.DefaultCircuitBreaker.ToString(),
                    Policy
                        .Handle<RpcException>()
                        .CircuitBreakerAsync(
                            2,
                            TimeSpan.FromSeconds(2),
                            (exception, span) =>
                            {
                                console.WriteLine($"Circuit broken. Span: {span}; Exception: {exception.Message};");
                            },
                            () =>
                            {
                                console.WriteLine("Circuit reset.");
                            }, () =>
                            {
                                console.WriteLine("Circuit half open.");
                            })
                },
            };
            
            return policyRegistry;
        }
    }
}