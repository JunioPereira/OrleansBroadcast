﻿using GransInterface.States;
using Microsoft.Extensions.Hosting;
using Orleans.BroadcastChannel;
using System.Diagnostics;
using System.Threading.Channels;

namespace SiloHost.Service
{
    public class StockWorker : BackgroundService
    {
        private readonly IBroadcastChannelProvider _provider;
        Dictionary<string, IBroadcastChannelWriter<StockState>> channelWrite { get; }
        string[] stockList { get; }
        Random _r { get; }

        public StockWorker(IClusterClient clusterClient)
        {
            _provider = clusterClient.GetBroadcastChannelProvider("stock");
            channelWrite = new Dictionary<string, IBroadcastChannelWriter<StockState>>();
            stockList = new string[] { "OIBR3", "PETR4", "ITUB4", "BBDC4", "AZUL4", "EMBR3" };
            _r = new Random();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                // Capture the starting timestamp.
                long startingTimestamp = Stopwatch.GetTimestamp();

                foreach (var stock in stockList)
                {
                    if (!channelWrite.ContainsKey(stock))
                    {
                        // Get the live stock ticker broadcast channel.
                        ChannelId channelId = ChannelId.Create("StockSubscription", stock);
                        IBroadcastChannelWriter<StockState> channelWriter = _provider.GetChannelWriter<StockState>(channelId);
                        channelWrite[stock] = channelWriter;
                    }

                    // Broadcast all stock updates on this channel.
                    await channelWrite[stock].Publish(new StockState() { Symbol = stock, Price = (decimal)_r.NextDouble() });
                }

                // Use the elapsed time to calculate a 15 second delay.
                int elapsed = Stopwatch.GetElapsedTime(startingTimestamp).Milliseconds;
                int remaining = Math.Max(0, 15_00 - elapsed);

                await Task.Delay(remaining, stoppingToken);
            }
        }
    }
}
