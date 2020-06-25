using System;
using System.Threading.Tasks;
using Autofac;
using CommandDotNet;
using CommandDotNet.Rendering;
using Grpc.Core;
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

        public static void AddDefaultPolicies(this ContainerBuilder builder)
        {
            builder
                .Register(GetDefaultRegistry)
                .As<IReadOnlyPolicyRegistry<string>>()
                .InstancePerLifetimeScope();
        }

        private static PolicyRegistry GetDefaultRegistry(IComponentContext componentContext)
        {
            var console = componentContext.Resolve<IConsole>();
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
                            TimeSpan.FromMilliseconds(100),
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
                        .WaitAndRetryAsync(Backoff.DecorrelatedJitterBackoffV2(
                            TimeSpan.FromMilliseconds(50),
                            MaxRetries), (exception, timeSpan, retryAttempt, context) =>
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
                        componentContext.Resolve<IAsyncCacheProvider>(),
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
                    Policy.TimeoutAsync(TimeSpan.FromMilliseconds(100), TimeoutStrategy.Pessimistic, (context, span, task) =>
                    {
                        // do not await, otherwise policy is useless.
                        task.ContinueWith(t =>
                        {
                            // ContinueWith important the abandoned task may still be executing, when the caller times out 

                            if (t.IsFaulted)
                            {
                                console.Out.WriteLine(
                                    $"Operation {context.OperationKey}: execution timed out after {span.TotalSeconds} seconds, eventually terminated with: {t.Exception}.");
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
                            }

                            // Additionally, clean up any resources ...
                        });
                        
                        console.Out.WriteLine($"Operation {context.OperationKey} timed out.");
                        return Task.CompletedTask;
                    })
                }
            };
            
            return policyRegistry;
        }
    }
}