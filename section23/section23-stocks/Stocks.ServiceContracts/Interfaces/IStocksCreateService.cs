using Stocks.ServiceContracts.DTOs;

namespace Stocks.ServiceContracts.Interfaces;

public interface IStocksCreateService
{
    Task<BuyOrderResponse> CreateBuyOrder(BuyOrderRequest? buyOrderRequest);

    Task<SellOrderResponse> CreateSellOrder(SellOrderRequest? sellOrderRequest);

}
