using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Section17.Assignment31.Models;
using Section16.Assignment29.ServiceContracts.DTOs;
using Section16.Assignment29.ServiceContracts.Interfaces;
using Section17.Assignment31.Web.Models;

namespace Section17.Assignment31.Controllers;

[Route("[Controller]")]
public class TradeController(
    IConfiguration configuration,
    IOptions<TradingOptions> options, 
    IFinnhubService finnhubService,
    IStocksService stocksService
    ) : Controller
{
    [Route("/{stockSymbol?}")]
    [Route("index/{stockSymbol?}")]
    [HttpGet]
    public async Task<IActionResult> Index(string? stockSymbol)
    {
        if (stockSymbol is null)
            stockSymbol = options.Value.DefaultStockSymbol;

        var stockQuote = await finnhubService.GetStockPriceQuote(stockSymbol);
        var profile = await finnhubService.GetCompanyProfile(stockSymbol);

        StockTrade stockTrade = new()
        {
            StockSymbol = stockSymbol,
            StockName = profile?["name"].ToString(),
            Price = Convert.ToDouble(stockQuote?["c"].ToString()),
        };
        ViewBag.Token = configuration["token"]?.ToString();
        ViewBag.DefaultOrderQuantity = options.Value.DefaultOrderQuantity;
        return View(stockTrade);
    }

    [Route("[Action]")]
    [HttpPost]
    public IActionResult BuyOrder(BuyOrderRequest buyOrderRequest)
    {
        ModelState.Remove("DateAndTimeOfOrder");
        if (!ModelState.IsValid)
        {
            return RedirectToAction("index", "trade", new { stockSymbol = buyOrderRequest .StockSymbol});
        }

        buyOrderRequest.DateAndTimeOfOrder = DateTime.Now;
        stocksService.CreateBuyOrder(buyOrderRequest);
        return RedirectToAction("orders");
    }

    [Route("[Action]")]
    [HttpPost]
    public IActionResult SellOrder(SellOrderRequest sellOrderRequest)
    {
        ModelState.Remove("DateAndTimeOfOrder");
        if (!ModelState.IsValid)
        {
                return RedirectToAction("index", "trade", new { stockSymbol = sellOrderRequest.StockSymbol });
        }

        sellOrderRequest.DateAndTimeOfOrder = DateTime.Now;
        stocksService.CreateSellOrder(sellOrderRequest);
        return RedirectToAction("orders");
    }


    [Route("[Action]")]
    [HttpGet]
    public async Task<IActionResult> Orders()
    {
        var buyOrders = await stocksService.GetBuyOrders();
        var sellOrders = await stocksService.GetSellOrders();
        Orders orders = new Orders() { BuyOrders = buyOrders, SellOrders = sellOrders };
        return View(orders);
    }
}
