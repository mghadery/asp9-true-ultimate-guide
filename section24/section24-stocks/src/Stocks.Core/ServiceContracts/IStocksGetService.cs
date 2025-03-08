using Stocks.Core.DTOs;

namespace Stocks.ServiceContracts.Interfaces;

public interface IStocksGetService
{
    Task<List<BuyOrderResponse>> GetBuyOrders();

    Task<List<SellOrderResponse>> GetSellOrders();
}
