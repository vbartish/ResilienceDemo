namespace ResilienceDemo.Battery
{
    public enum RetryPolicyKey
    {
        NoRetry,
        BasicRetryOnRpc,
        RetryOnRpcWithExponentialBackoff,
        RetryOnRpcWithJitter
    }

    public enum CachePolicyKey
    {
        NoCache,
        InMemoryCache
    }
}