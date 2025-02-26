using Microsoft.AspNetCore.Mvc;
using Stocks.ServiceContracts.Interfaces;
using Stocks.Web.Models;

namespace Stocks.Web.ViewComponents;

public class StockViewComponent(IFinnhubService finnhubService) :ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync(string stockSymbol)
    {
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
        return View(stockDetails);
    }
}
