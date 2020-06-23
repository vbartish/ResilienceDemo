using System;
using System.Threading.Tasks;
using Autofac;
using CommandDotNet;
using CommandDotNet.Rendering;
using Grpc.Core;
using Polly;
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
                    PolicyKey.BasicRetryOnRpc.ToString(), Policy
                        .Handle<RpcException>()
                        .RetryAsync(MaxRetries, (exception, retryAttempt, context) =>
                        {
                            console.Out.WriteLine($"Attempt {retryAttempt - 1} failed: {exception.Message}. Retrying.");
                            return Task.CompletedTask;
                        })
                },
                {
                    PolicyKey.RetryOnRpcWithExponentialBackoff.ToString(), Policy
                        .Handle<RpcException>()
                        .WaitAndRetryAsync(Backoff.ExponentialBackoff(
                            TimeSpan.FromMilliseconds(100),
                            MaxRetries), (exception, timeSpan, retryAttempt, context) =>
                        {
                            console.Out.WriteLine(
                                $"TimeSpan: {timeSpan.ToString()}. Attempt {retryAttempt - 1} failed: {exception.Message}. Retrying.");
                            return Task.CompletedTask;
                        })
                },
                {
                    PolicyKey.RetryOnRpcWithJitter.ToString(), Policy
                        .Handle<RpcException>()
                        .WaitAndRetryAsync(Backoff.DecorrelatedJitterBackoffV2(
                            TimeSpan.FromMilliseconds(100),
                            MaxRetries), (exception, timeSpan, retryAttempt, context) =>
                        {
                            console.Out.WriteLine(
                                $"TimeSpan: {timeSpan.ToString()}. Attempt {retryAttempt - 1} failed: {exception.Message}. Retrying.");
                            return Task.CompletedTask;
                        })
                }
            };
            
            return policyRegistry;
        }
    }
}