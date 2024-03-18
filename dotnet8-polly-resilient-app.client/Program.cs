
using System.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using static System.Console;
HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);
builder.Logging.ClearProviders();

IServiceCollection services = builder.Services;
services.AddHttpClient("weather", client => client.BaseAddress = new Uri("http://localhost:5197"));


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
        try 
        {
            var response = await httpClient.GetAsync("/weatherforecast");
            WriteLine($"{(int)response.StatusCode}: {watch.Elapsed.TotalMilliseconds,10:0.00},ms");
        }
        catch(Exception ex) 
        {  
            WriteLine($"Err: {watch.Elapsed.TotalMilliseconds, 10:0.00}ms ({ex.GetType().Name})");
        }
    });
    
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