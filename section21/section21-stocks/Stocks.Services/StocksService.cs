using Stocks.Entities;
using Stocks.Helpers.Validators;
using Stocks.ServiceContracts.DTOs;
using Stocks.ServiceContracts.Interfaces;

namespace Stocks.Services;

public class StocksService(IStocksRepo<BuyOrder> buyRepo, IStocksRepo<SellOrder> sellRepo) : IStocksService
{
    public async Task<BuyOrderResponse> CreateBuyOrder(BuyOrderRequest? buyOrderRequest)
    {
        ModelValidator.IsValid(buyOrderRequest);

        BuyOrder buyOrder = (BuyOrder)buyOrderRequest;
        buyOrder.BuyOrderID = Guid.NewGuid();
        var r = await buyRepo.Add(buyOrder);

        BuyOrderResponse buyOrderResponse = (BuyOrderResponse)buyOrder;
        return buyOrderResponse;
    }

    public async Task<SellOrderResponse> CreateSellOrder(SellOrderRequest? sellOrderRequest)
    {
        ModelValidator.IsValid(sellOrderRequest);

        SellOrder sellOrder = (SellOrder)sellOrderRequest;
        sellOrder.SellOrderID = Guid.NewGuid();
        var r = await sellRepo.Add(sellOrder);

        SellOrderResponse sellOrderResponse = (SellOrderResponse)sellOrder;
        return sellOrderResponse;
    }

    public async Task<List<BuyOrderResponse>> GetBuyOrders()
    {
        var r = (await buyRepo.GetAll()).Select(x=> (BuyOrderResponse)(x))
            .OrderByDescending(x=> x.DateAndTimeOfOrder)
            .ToList();
        return r;
    }

    public async Task<List<SellOrderResponse>> GetSellOrders()
    {
        var r = (await sellRepo.GetAll()).Select(x => (SellOrderResponse)(x))
            .OrderByDescending(x => x.DateAndTimeOfOrder)
            .ToList();
        return r;
    }
}
