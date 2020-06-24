namespace ResilienceDemo.Battery
{
    public enum RetryPolicyKey
    {
        BasicRetryOnRpc,
        RetryOnRpcWithExponentialBackoff,
        RetryOnRpcWithJitter
    }

    public enum CachePolicyKey
    {
        NoOp,
        InMemoryCache
    }
}