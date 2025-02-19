using Section16.Assignment29.ServiceContracts.DTOs;

namespace Section16.Assignment29.ServiceContracts.Interfaces;

public interface IStocksService
{
    Task<BuyOrderResponse> CreateBuyOrder(BuyOrderRequest? buyOrderRequest);

    Task<SellOrderResponse> CreateSellOrder(SellOrderRequest? sellOrderRequest);

    Task<List<BuyOrderResponse>> GetBuyOrders();

    Task<List<SellOrderResponse>> GetSellOrders();
}
