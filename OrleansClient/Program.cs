// See https://aka.ms/new-console-template for more information
using GransInterface.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

Console.WriteLine("Hello, World!");


var host = await StartOrleansClient();
var client = host.Services.GetRequiredService<IClusterClient>();


Console.WriteLine("");
Console.WriteLine("");
Console.WriteLine("");
Console.WriteLine("");

string _exit = string.Empty;

try
{
    while (_exit != "S") 
    {
        Console.WriteLine("");
        Console.WriteLine("Input account!");
        Console.WriteLine("");

        int account = int.Parse(Console.ReadLine());

        var igrain = client.GetGrain<ICustomerGrain>(account);

        var a = await igrain.Get();
    }
}
finally 
{
    await host.StopAsync();
}








static async Task<IHost> StartOrleansClient()
{
    var client = new HostBuilder()
        .UseOrleansClient(c =>
        {
            c.UseLocalhostClustering();
        })
        .ConfigureLogging(l =>
        {
            l.SetMinimumLevel(LogLevel.Warning).AddConsole();
        });

    var host = client.Build();
    await host.StartAsync();
    return host;
}
