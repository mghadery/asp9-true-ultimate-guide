using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Stocks.ServiceContracts.Interfaces;
using Rotativa.AspNetCore;
using Stocks.Web.Filters;
using Stocks.Core.DTOs;
using Stocks.Core.FinnhubServiceContracts;

namespace Stocks.Controllers;

[Route("[Controller]")]
public class TradeController(
    IConfiguration configuration,
    IOptions<TradingOptions> options,
    IFinnhubService finnhubService,
    IStocksGetService stocksGetService,
    IStocksCreateService stocksCreateService,
    ILogger<TradeController> logger
    ) : Controller
{
    [Route("/{stockSymbol?}")]
    [Route("index/{stockSymbol?}")]
    [HttpGet]
    public async Task<IActionResult> Index(string? stockSymbol)
    {
        logger.LogInformation(
            "Action: {0} in controller: {1} with stockSymbol: {2} started",
            ControllerContext.ActionDescriptor.ActionName,
            ControllerContext.ActionDescriptor.ControllerName,
            stockSymbol);

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
    [TypeFilter(typeof(ValidateRequestFilter))]
    public async Task<IActionResult> BuyOrder(BuyOrderRequest request)
    {
        //throw new ArgumentException("This is an intentional error to test the handler");
        //ModelState.Remove("DateAndTimeOfOrder");
        //if (!ModelState.IsValid)
        //{
        //    return RedirectToAction("index", "trade", new { stockSymbol = request.StockSymbol });
        //}

        //request.DateAndTimeOfOrder = DateTime.Now;
        await stocksCreateService.CreateBuyOrder(request);
        return RedirectToAction("orders");
    }

    [Route("[Action]")]
    [HttpPost]
    [TypeFilter(typeof(ValidateRequestFilter))]
    public async Task<IActionResult> SellOrder(SellOrderRequest request)
    {
        //ModelState.Remove("DateAndTimeOfOrder");
        //if (!ModelState.IsValid)
        //{
        //    return RedirectToAction("index", "trade", new { stockSymbol = request.StockSymbol });
        //}

        //request.DateAndTimeOfOrder = DateTime.Now;
        await stocksCreateService.CreateSellOrder(request);
        return RedirectToAction("orders");
    }


    [Route("[Action]")]
    [HttpGet]
    public async Task<IActionResult> Orders()
    {
        var buyOrders = await stocksGetService.GetBuyOrders();
        var sellOrders = await stocksGetService.GetSellOrders();
        Orders orders = new Orders() { BuyOrders = buyOrders, SellOrders = sellOrders };
        return View(orders);
    }

    [Route("[Action]")]
    [HttpGet]
    public async Task<IActionResult> OrdersAsPdf()
    {
        var buyOrders = (await stocksGetService.GetBuyOrders()).Select(x => (IOrderResponse)x).ToList();
        var sellOrders = (await stocksGetService.GetSellOrders()).Select(x => (IOrderResponse)x).ToList();
        var orders = buyOrders.Union(sellOrders).OrderBy(x => x.DateAndTimeOfOrder).ToList();
        return new ViewAsPdf("ordersAsPdf", orders, ViewData);
    }
}
