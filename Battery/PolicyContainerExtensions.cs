using System;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;
using Autofac;
using CommandDotNet;
using CommandDotNet.Rendering;
using Grpc.Core;
using GrpcDivisionControlUnit;
using Polly;
using Polly.Caching;
using Polly.Contrib.WaitAndRetry;
using Polly.Registry;

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
                    RetryPolicyKey.BasicRetryOnRpc.ToString(), Policy
                        .Handle<RpcException>()
                        .RetryAsync(MaxRetries, (exception, retryAttempt, context) =>
                        {
                            console.Out.WriteLine($"Operation: {context.OperationKey}; Attempt {retryAttempt - 1} failed: {exception.Message}. Retrying.");
                            return Task.CompletedTask;
                        })
                },
                {
                    RetryPolicyKey.RetryOnRpcWithExponentialBackoff.ToString(), Policy
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
                    RetryPolicyKey.RetryOnRpcWithJitter.ToString(), Policy
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
                    Policy.CacheAsync(
                        componentContext.Resolve<IAsyncCacheProvider>(),
                        TimeSpan.FromMinutes(5),
                        (policyContext, cacheKey) => console.WriteLine($"Operation {policyContext.OperationKey}: Cache get {cacheKey}"),
                        (policyContext, cacheKey) => console.WriteLine($"Operation {policyContext.OperationKey}: Cache miss {cacheKey}"),
                        (policyContext, cacheKey) => console.WriteLine($"Operation {policyContext.OperationKey}: Cache put {cacheKey}"),
                        (policyContext, cacheKey, exception) => console.WriteLine($"Operation {policyContext.OperationKey}: Cache get error {cacheKey}; {exception}"),
                        (policyContext, cacheKey, exception) => console.WriteLine($"Operation {policyContext.OperationKey}: Cache put error {cacheKey}; {exception}"))
                }
                ,
                {
                    CachePolicyKey.NoOp.ToString(),
                    Policy.NoOpAsync<Meteo>()
                }
            };
            
            return policyRegistry;
        }
    }
}