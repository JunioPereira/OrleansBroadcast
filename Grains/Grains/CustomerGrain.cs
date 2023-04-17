using GransInterface.Interfaces;
using GransInterface.States;
using Orleans.Providers;

namespace Grains.Grains
{
    [StorageProvider(ProviderName = "customer")]
    public class CustomerGrain : Grain<CustomerState>, ICustomerGrain
    {
        public override Task OnActivateAsync(CancellationToken cancellationToken)
        {
            return base.OnActivateAsync(cancellationToken);
        }

        public override Task OnDeactivateAsync(DeactivationReason reason, CancellationToken cancellationToken)
        {
            return base.OnDeactivateAsync(reason, cancellationToken);
        }

        public Task<CustomerState> Get()
        {
            return Task.FromResult(State);
        }

        public Task Set(CustomerState customer)
        {
            State = customer;
            return Task.CompletedTask;
        }
    }
}
