using Section16.Assignment29.Entities;
using Section16.Assignment29.Helpers.Validators;
using Section16.Assignment29.ServiceContracts.DTOs;
using Section16.Assignment29.ServiceContracts.Interfaces;

namespace Section16.Assignment29.Services;

public class StocksService : IStocksService
{
    private readonly List<BuyOrder> _buyOrders = new();
    private readonly List<SellOrder> _sellOrders = new();
    public async Task<BuyOrderResponse> CreateBuyOrder(BuyOrderRequest? buyOrderRequest)
    {
        ModelValidator.IsValid(buyOrderRequest);

        BuyOrder buyOrder = (BuyOrder)buyOrderRequest;
        buyOrder.BuyOrderID = Guid.NewGuid();
        _buyOrders.Add(buyOrder);

        BuyOrderResponse buyOrderResponse = (BuyOrderResponse)buyOrder;
        return buyOrderResponse;
    }

    public async Task<SellOrderResponse> CreateSellOrder(SellOrderRequest? sellOrderRequest)
    {
        ModelValidator.IsValid(sellOrderRequest);

        SellOrder sellOrder = (SellOrder)sellOrderRequest;
        sellOrder.SellOrderID = Guid.NewGuid();
        _sellOrders.Add(sellOrder);

        SellOrderResponse sellOrderResponse = (SellOrderResponse)sellOrder;
        return sellOrderResponse;
    }

    public async Task<List<BuyOrderResponse>> GetBuyOrders()
    {
        var r = _buyOrders.Select(x=> (BuyOrderResponse)(x)).ToList();
        return r;
    }

    public async Task<List<SellOrderResponse>> GetSellOrders()
    {
        var r = _sellOrders.Select(x => (SellOrderResponse)(x)).ToList();
        return r;
    }
}
