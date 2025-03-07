using Stocks.ServiceContracts.DTOs;

namespace Stocks.ServiceContracts.Interfaces;

public interface IStocksGetService
{
    Task<List<BuyOrderResponse>> GetBuyOrders();

    Task<List<SellOrderResponse>> GetSellOrders();
}
