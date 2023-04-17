using GransInterface.States;
using Orleans;

namespace GransInterface.Interfaces
{
    public interface ICustomerGrain:IGrainWithIntegerKey
    {
        Task<CustomerState> Get();
        Task Set(CustomerState customer);
    }
}
