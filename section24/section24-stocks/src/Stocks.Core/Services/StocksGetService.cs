using Stocks.Core.Domain.Entities;
using Stocks.Core.Domain.RepositoryContracts;
using Stocks.Core.DTOs;
using Stocks.ServiceContracts.Interfaces;

namespace Stocks.Core.Services;

public class StocksGetService(IStocksRepo<BuyOrder> buyRepo, IStocksRepo<SellOrder> sellRepo) : IStocksGetService
{
    public async Task<List<BuyOrderResponse>> GetBuyOrders()
    {
        var r = (await buyRepo.GetAll()).Select(x => (BuyOrderResponse)x)
            .OrderByDescending(x => x.DateAndTimeOfOrder)
            .ToList();
        return r;
    }

    public async Task<List<SellOrderResponse>> GetSellOrders()
    {
        var r = (await sellRepo.GetAll()).Select(x => (SellOrderResponse)x)
            .OrderByDescending(x => x.DateAndTimeOfOrder)
            .ToList();
        return r;
    }
}
