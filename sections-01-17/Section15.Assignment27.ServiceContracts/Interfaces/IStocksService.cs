using Section15.Assignment27.ServiceContracts.DTOs;

namespace Section15.Assignment27.ServiceContracts.Interfaces;

public interface IStocksService
{
    Task<BuyOrderResponse> CreateBuyOrder(BuyOrderRequest? buyOrderRequest);

    Task<SellOrderResponse> CreateSellOrder(SellOrderRequest? sellOrderRequest);

    Task<List<BuyOrderResponse>> GetBuyOrders();

    Task<List<SellOrderResponse>> GetSellOrders();
}
