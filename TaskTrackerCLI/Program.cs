using Adapters.Output.Console;
using Adapters.Persistence.JsonFile;
using Core.Ports;
using Core.UseCases;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TaskTrackerHost.CLI;


IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext,services) =>
    {
        IConfiguration config = hostContext.Configuration;

        services.AddTransient<Driver>(provider => new Driver(
            provider.GetRequiredService<ICommandProcessor>(),
            args
        ));
        services.AddTransient<ICommandProcessor, CommandProcessor>();
        services.AddTransient<IOutputPort, ConsoleOutputAdapter>();
        services.AddTransient<IPersistencePort, JsonFilePersistenceAdapter>();
    }).Build();

Driver d = host.Services.GetRequiredService<Driver>();
await d.RunAsync(default);