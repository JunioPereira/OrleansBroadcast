using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SiloHost.Service;

namespace SiloHost
{
    public class SiloHostService : IHostedService
    {
        IHost? SiloHost { get; set; }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            return StartSilo(cancellationToken);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        private async Task StartSilo(CancellationToken cancellationToken)
        {
            SiloHost = new HostBuilder()
                .UseOrleans(o => 
                {
                    o.UseLocalhostClustering();

                    o.AddMemoryGrainStorage("customer");
                    o.AddMemoryGrainStorage("stock");

                    o.AddBroadcastChannel(
                                "stock",
                                options => options.FireAndForgetDelivery = true);

                    o.UseDashboard(d => { 
                        d.Port = 8081; 
                    });
                    

                })
                .ConfigureLogging(l => { 
                    l.SetMinimumLevel(LogLevel.Warning).AddConsole(); 
                })
                .ConfigureServices(service => { service.AddHostedService<StockWorker>(); })
                .Build();

            await SiloHost.StartAsync(cancellationToken);
        }
    }
}
