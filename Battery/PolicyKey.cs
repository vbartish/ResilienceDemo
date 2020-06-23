namespace ResilienceDemo.Battery
{
    public enum PolicyKey
    {
        BasicRetryOnRpc,
        RetryOnRpcWithExponentialBackoff,
        RetryOnRpcWithJitter
    }
}