using Stocks.Core.Domain.Entities;
using Stocks.Core.Domain.RepositoryContracts;
using Stocks.Core.DTOs;
using Stocks.Core.Validators;
using Stocks.ServiceContracts.Interfaces;

namespace Stocks.Core.Services;

public class StocksCreateService(IStocksRepo<BuyOrder> buyRepo, IStocksRepo<SellOrder> sellRepo) : IStocksCreateService
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
}
