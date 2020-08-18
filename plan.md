# Intro
1. Define the problem. 
    * Failures are given. Most of the time our system work in partially faulted state. 
    * Scary diagram with several services/components, show how do they communicate, and typical failures.
2. Define resiliency as the trait of the system.
    * https://www.oxfordlearnersdictionaries.com/us/definition/english/resilience#:~:text=%E2%80%8Bthe%20ability%20of%20people,position%20to%20win%20the%20game.
    * in software development we usually mean an ability of the system to recover from fault and provide 
    best service possible.
3. Resiliency could and should be a concern at all "levels" of the software system. You should build your infrastructure, your back-end services and apps and user-facing apps with resiliency in mind.
4. Today we will talk about application resiliency. So let's leave infrastructure aside. We will focus on .Net Core however almost everything will still be applicable for .Net Framework. We will focus on back-end implementations, however same principles are applicable for almost any kind of application.

## Retry
1. basic approach - simple retry with Polly. Show a problem with it. 
2. improved approach - retry with back-off. Show polly example. Describe a potential problem.
3. advanced approach - retry with jitter.
 
## Timeout
1. why use Polly, or custom implementation when you can use default timeouts on DB connections or service clients? 
2. improve previous demo or show another endpoint usage with timeout

## Cache
1. why Polly?
2. Distributed cache?

## Fallback 
1. show some visual or demo the problem
2. improve previous demo by adding a fallback policy

## Circuit Breaker (use on client when bulkhead is on server)
1. basic approach - simple circuit breaker with Polly
2. distributed breaker?

## Bulkhead (use on client when circuit breaker is on server)

1. show some visual or demo the problem
2. improve previous demo by adding bulkhead to the service.

# Testing resiliency
1. Simmy.
2. Chaos engineering.
