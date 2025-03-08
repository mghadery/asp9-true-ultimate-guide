using Microsoft.AspNetCore.Mvc;
using Stocks.Core.DTOs;
using Stocks.Core.FinnhubServiceContracts;

namespace Stocks.Web.ViewComponents;

public class StockViewComponent(IFinnhubService finnhubService, ILogger<StockViewComponent> logger) :ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync(string stockSymbol)
    {
        logger.LogInformation("StockViewComponent started with stockSymbol: {@stockSymbol}", stockSymbol);

        ViewData.Clear();

        var profile = await finnhubService.GetCompanyProfile(stockSymbol);
        var stockQuote = await finnhubService.GetStockPriceQuote(stockSymbol);
        if (profile is null)
        {
            ViewBag.Error = "Error getting profile";
            return View();
        }
        StockDetails stockDetails = new()
        {
            Symbol = stockSymbol,
            Name = profile["name"].ToString(),
            FinnhubIndustry = profile["finnhubIndustry"].ToString(),
            Exchange = profile["exchange"].ToString(),
            Currency = profile["currency"].ToString(),
            Logo = profile["logo"].ToString(),
            Price = Convert.ToDouble(stockQuote?["c"].ToString())
        };
        logger.LogInformation("StockViewComponent stockDetails: {@stockDetails}", stockDetails);
        return View(stockDetails);
    }
}
