using CommandDotNet.Rendering;
using Grpc.Core;
using Grpc.Core.Interceptors;
using Polly;
using Polly.Registry;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ResilienceDemo.Battery
{
    internal class ResiliencyInterceptor : Interceptor
    {
        private IReadOnlyPolicyRegistry<string> _policyRegistry;
        private readonly IConsole console;

        public ResiliencyInterceptor(IReadOnlyPolicyRegistry<string> policyRegistry, IConsole console)
        {
            _policyRegistry = policyRegistry;
            this.console = console;
        }

        public override AsyncUnaryCall<TResponse> AsyncUnaryCall<TRequest, TResponse>(TRequest request, ClientInterceptorContext<TRequest, TResponse> context, AsyncUnaryCallContinuation<TRequest, TResponse> continuation)
        {
            var policies = context.Options.Headers.Get(HeaderKeys.Policies)?.Value?.Split(HeaderKeys.Separator);
            IAsyncPolicy executionPolicy = Policy.NoOpAsync();

            if (policies != null)
            {
                foreach (var policyKey in policies)
                {
                    executionPolicy = executionPolicy.WrapAsync(_policyRegistry.Get<IAsyncPolicy>(policyKey.ToString()) ?? Policy.NoOpAsync());
                }
            }

            var headerTask = new TaskCompletionSource<Metadata>();
            var status = default(Status);
            var trailers = default(Metadata);
            var cts = new CancellationTokenSource();

            return new AsyncUnaryCall<TResponse>(
                Run(),
                headerTask.Task,
                () => status,
                () => trailers,
                () => cts.Cancel());

            async Task<TResponse> Run()
            { 
                using(var cancel = CancellationTokenSource.CreateLinkedTokenSource(cts.Token, context.Options.CancellationToken))
                {
                    var policyResult = await executionPolicy.ExecuteAndCaptureAsync(async _ =>
                    {
                        if (cancel.Token.IsCancellationRequested)
                        {
                            throw new RpcException(Status.DefaultCancelled);
                        }

                        var newMetadata = new Metadata();
                        context.Options.Headers
                        .Where(entry => entry.Key != HeaderKeys.Policies && entry.Key != HeaderKeys.OperationKey)
                        .ToList()
                        .ForEach(entry => newMetadata.Add(entry));


                        var newContext = new ClientInterceptorContext<TRequest, TResponse>(
                            context.Method,
                            context.Host,
                            new CallOptions(
                                newMetadata,
                                Defaults.DefaultGrpcDeadline,
                                context.Options.CancellationToken,
                                context.Options.WriteOptions,
                                context.Options.PropagationToken,
                                context.Options.Credentials));

                        var asyncCall = continuation(request, newContext);
                        cancel.Token.Register(() =>
                        {
                            asyncCall.Dispose();
                        });

                        var result = await asyncCall;

                        headerTask.SetResult(await asyncCall.ResponseHeadersAsync);
                        status = asyncCall.GetStatus();
                        trailers = asyncCall.GetTrailers();

                        return result;
                    }, new Context(context.Options.Headers.GetValue(HeaderKeys.OperationKey) ?? string.Empty));

                    if (policyResult.Outcome != OutcomeType.Successful)
                    {
                        throw policyResult.FinalException;
                    }

                    return policyResult.Result;
                }
            }
        }
    }
}