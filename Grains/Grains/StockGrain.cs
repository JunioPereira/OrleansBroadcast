using GransInterface.Interfaces;
using GransInterface.States;
using Orleans.BroadcastChannel;
using Orleans.Providers;

namespace Grains.Grains
{
    [StorageProvider(ProviderName = "stock")]
    [ImplicitChannelSubscription("StockSubscription")]
    public class StockGrain : Grain<StockState>, IStockGrain, IOnBroadcastChannelSubscribed
    {
        public override Task OnActivateAsync(CancellationToken cancellationToken)
        {
            State.Symbol = this.GetPrimaryKeyString();

            return base.OnActivateAsync(cancellationToken);
        }

        public override Task OnDeactivateAsync(DeactivationReason reason, CancellationToken cancellationToken)
        {
            return base.OnDeactivateAsync(reason, cancellationToken);
        }
        public Task OnSubscribed(IBroadcastChannelSubscription streamSubscription)
        {
            return streamSubscription.Attach<StockState>(OnStockUpdated, OnError);
        }

        public Task<StockState> Get()
        {
            return Task.FromResult(State);
        }

        public Task Set(decimal price)
        {
            State.Price = price;
            WriteStateAsync();

            return Task.CompletedTask;
        }

        private Task OnStockUpdated(StockState stock)
        {
            State = stock;
            return Task.CompletedTask;
        }

        private static Task OnError(Exception ex)
        {
            Console.Error.WriteLine($"An error occurred: {ex}");

            return Task.CompletedTask;
        }
    }
}
