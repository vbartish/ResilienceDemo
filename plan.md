# Intro
1. Define the problem. 
    * Failures are given. Most of the time our system work in partially faulted state. 
    * Scary diagram with several services/components, show how do they communicate, and typical failures.
2. Define resiliency as the trait of the system.
    * https://www.oxfordlearnersdictionaries.com/us/definition/english/resilience#:~:text=%E2%80%8Bthe%20ability%20of%20people,position%20to%20win%20the%20game.
    * in software development we usually mean an ability of the system to recover from fault and provide 
    best service possible.
3. Resiliency could and should be a concern an all "levels" of the software system. You should build your infrastructure, your back-end services and apps and user-facing apps with resiliency in mind.
4. Today we will talk about application resiliency. So let's leave infrastructure aside. We will focus on .Net Core however almost everything will still be applicable for .Net Framework. We will focus on back-end implementations, however same principles are applicable for almost any kind of application.

# Reactive resiliency patterns

## Retry (done)
1. show some visual or demo the problem 
2. basic approach - simple retry with Polly. Show a problem with it. 
3. improved approach - retry with back-off. Show polly example. Describe a potential problem.
4. advanced approach - retry with jitter. 

## Circuit Breaker (use on client when bulkhead is on server)
1. show some visual or demo the problem
2. basic approach - simple circuit breaker with Polly
3. improve previous demo by wrapping retry policy with circuit breaker
4. distributed breaker?

## Fall back (combine with timeout for correction)
1. show some visual or demo the problem
2. improve previous demo by adding a fallback policy

# Proactive resiliency patterns

## Bulkhead (use on client when circuit breaker is on server)

1. show some visual or demo the problem
2. improve previous demo by adding bulkhead to the service.

## Timeout (done)
1. show some visual or demo the problem
2. why use Polly, or custom implementation when you can use default timeouts on DB connections or service clients? 
3. improve previous demo or show another endpoint usage with timeout

## Cache (done)
1. show some visual or demo the problem
2. why Polly?
3. Distributed cache? 

# Testing resiliency (?)
1. NoOp policy? 
2. Simmy?



