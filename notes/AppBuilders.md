# App Builders

- [Content index](../README.md)
- [Polly Extensions](./pollyNotes.md)
- [Libraries used in the sample](./UsedLibs.md)

## Host.CreateDefaultBuilder

Custom Configuration: If you need to customize the configuration of your host beyond what ```Host.CreateDefaultBuilder()``` provides, you can use it to get a clean instance of HostBuilder and configure it according to your requirements.

It provides a default set of configurations and services appropriate for most applications but allows customization through method chaining.

## WebApplication.CreateBuilder

WebApplication.CreateBuilder is used specifically to create an instance of a web application withing the ASP,NET Core framework.

It configures the web application's environment, including Kestrel (the web server), routing, middleware pipeline and other web-specific configurations.

it's built on to of ```Host.CreateDefaultBuilder```, so it includes all the configurations provided in the default builder in addition to web-specific configurations.

