
using System.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Http.Resilience;
using Microsoft.Extensions.Logging;
using Polly;
using static System.Console;
HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);
builder.Logging.ClearProviders();

IServiceCollection services = builder.Services;
services.AddHttpClient("weather", client => client.BaseAddress = new Uri("http://localhost:5197"))
    .AddResilienceHandler("demo", builder => 
    {
        builder.AddRetry(new HttpRetryStrategyOptions
        {
            MaxRetryAttempts = 5,
            BackoffType = DelayBackoffType.Exponential,
            UseJitter = true,
            Delay = TimeSpan.Zero
        });
        builder.AddTimeout(TimeSpan.FromSeconds(1));
    });


// Getting Services
var collectionServices = builder.Build().Services;
var httpClient = collectionServices
    .GetRequiredService<IHttpClientFactory>()
    .CreateClient("weather");

var watch = Stopwatch.StartNew();
var executionIndex = 1;
while(true) 
{

    await BatchAsync(async () => 
    {
        var watch = Stopwatch.StartNew();
        try 
        {
            var response = await httpClient.GetAsync("/weatherforecast");
            var statusCode = (int)response.StatusCode;

            if (statusCode is 500)
            {
                WriteRedLine($"{(int)response.StatusCode}: {watch.Elapsed.TotalMilliseconds,10:0.00},ms");
            }

            WriteLine($"{(int)response.StatusCode}: {watch.Elapsed.TotalMilliseconds,10:0.00},ms");
        }
        catch(Exception ex) 
        {  
            WriteRedLine($"Err: {watch.Elapsed.TotalMilliseconds, 10:0.00}ms ({ex.GetType().Name})");
        }
        finally
        {
            watch.Stop();    
        }
    });
    
}


void WriteRedLine(string message) 
{
    ForegroundColor = ConsoleColor.Red;
    WriteLine(message);
    ResetColor();
}

async Task BatchAsync(Func<Task> callback) 
{
    WriteLine($"****** Execution: {executionIndex} *******");
    for(int i = 0; i < 10; i++) 
    {
        await callback();
    }
    WriteLine($"****************************\n");
    executionIndex++; 
    Thread.Sleep(6000);
}