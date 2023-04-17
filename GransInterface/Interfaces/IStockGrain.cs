using GransInterface.States;

namespace GransInterface.Interfaces
{
    public interface IStockGrain : IGrainWithStringKey
    {
        Task<StockState> Get();
        Task Set(decimal price);
    }
}
