# Polly 
## Creation using Pipeline - Specific pipeline

We use  **ResiliancePipeline** to create an instance that will help use execute any (async, sync) or even something that returns void, and with the builder instance we can apply as many strategies as you needed

``` 

ResiliencePipeline newInstance = new ResiliencePipelineBuilder()
    .AddRetry(new RetryStrategyOptions
    {
        MaxRetryAttempts = 4,
        BackoffType = DelayBackoffType.Exponential,
        UseJitter = true
    })
    .AddTimeout(TimeSpan.FromSeconds(3))
    .Build();

```

You use the new instance to execute any callback.

```
await newInstance.ExecutreAsync(
    async token => 
    {
        // you can put your code logic here
    }, cancellationToken);
```

## For Non-Specific generic pipelines, use generic pipeline

You add ``` ResilencePipeline<Type> ``` to specify which will be your strategy and which type of callbacks will be handled.

``` 
ResiliencePipeline<HttpResponseMessage> httpInstancePipe = new ResiliencePipelineBuilder<HttpResponseMessage>()
    .AddRetry(new RetryStrategyOptions<HttpResonseMessage>
    {
        ShouldHandle = new PredicateBuilder<HttpResonseMessage>().
            .Handle<HttpResonseMessage>()
            .HandleResult(response => response.StatusCode => HttpStatusCode.InternalServerError)
        MaxRetryAttempts = 4,
        BackoffType = DelayBackoffType.Exponential,
        UseJitter = true
    })
    .AddTimeout(TimeSpan.FromSeconds(3))
    .Build();
```

And the you could use the pipe for all your http calls.


```
await httpInstancePipe.ExecutreAsync(
    async token => await httpClient.GetAsync("/some-path"), cancellationToken);
```

## Http polly resilience libraries

```Microsoft.Extensions.Http.Resilience``` is an API for building and using HTTP resilience pipelines.

- Custom pipelines
- Standrard pipelines
- Standar hedging pipeline

```Microsoft.Extensions.Resilience``` is a thin library that enriches the built-in Telemetry of poly

