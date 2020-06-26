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
    
    public enum TimeoutPolicyKey
    {
        NoTimeout, 
        DefaultPessimisticTimeout,
        DefaultOptimisticTimeout
    }
    
    public enum CircuitBreakerPolicyKey
    {
        NoBreaker, 
        DefaultCircuitBreaker
    }
}