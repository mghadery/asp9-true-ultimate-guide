using Microsoft.EntityFrameworkCore;
using Stocks.Entities;
using Stocks.Persistent;
using Stocks.ServiceContracts.DTOs;
using Stocks.ServiceContracts.Interfaces;
using Stocks.Services;
using Xunit.Abstractions;

namespace Stocks.Tests;

public class StocksServiceTests
{
    private readonly IStocksCreateService _stocksCreateService;
    private readonly IStocksGetService _stocksGetService;
    private readonly ITestOutputHelper _testOutputHelper;

    public StocksServiceTests(ITestOutputHelper testOutputHelper)
    {
        IStocksRepo<BuyOrder> buyRepo = new StocksRepo<BuyOrder>(new StocksDbContext(new DbContextOptionsBuilder().Options));
        IStocksRepo<SellOrder> sellRepo = new StocksRepo<SellOrder>(new StocksDbContext(new DbContextOptionsBuilder().Options));
        _stocksCreateService = new StocksCreateService(buyRepo, sellRepo);
        _stocksGetService = new StocksGetService(buyRepo, sellRepo);
        _testOutputHelper = testOutputHelper;
    }
    #region CreateBuyOrder
    //1. When you supply BuyOrderRequest as null, it should throw ArgumentNullException.
    [Fact]
    public async Task CreateBuyOrder_NullRequest()
    {
        //Arrange
        BuyOrderRequest buyOrderRequest = null;

        //Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => _stocksCreateService.CreateBuyOrder(buyOrderRequest));

    }

    //2. When you supply buyOrderQuantity as 0 (as per the specification, minimum is 1), it should throw ArgumentException.
    [Fact]
    public async Task CreateBuyOrder_Quantity0()
    {
        //Arrange
        BuyOrderRequest? buyOrderRequest = new BuyOrderRequest
        {
            DateAndTimeOfOrder = DateTime.Parse("2001-12-31"),
            Price = 10,
            Quantity = 0,
            StockName = "Microsoft",
            StockSymbol = "MSFT"
        };

        //Assert
        await Assert.ThrowsAsync<ArgumentException>(async () =>
            //Act
            await _stocksCreateService.CreateBuyOrder(buyOrderRequest));
    }


    //3. When you supply buyOrderQuantity as 100001 (as per the specification, maximum is 100000), it should throw ArgumentException.

    //4. When you supply buyOrderPrice as 0 (as per the specification, minimum is 1), it should throw ArgumentException.

    //5. When you supply buyOrderPrice as 10001 (as per the specification, maximum is 10000), it should throw ArgumentException.

    //6. When you supply stock symbol=null (as per the specification, stock symbol can't be null), it should throw ArgumentException.

    //7. When you supply dateAndTimeOfOrder as "1999-12-31" (YYYY-MM-DD) - (as per the specification, it should be equal or newer date than 2000-01-01), it should throw ArgumentException.
    [Fact]
    public async Task CreateBuyOrder_InvalidDate()
    {
        //Arrange
        BuyOrderRequest? buyOrderRequest = new BuyOrderRequest
        {
            DateAndTimeOfOrder = DateTime.Parse("1999-12-31"),
            Price = 10,
            Quantity = 1,
            StockName = "Microsoft",
            StockSymbol = "MSFT"
        };

        //Assert
        await Assert.ThrowsAsync<ArgumentException>(async () =>
            //Act
            await _stocksCreateService.CreateBuyOrder(buyOrderRequest));
    }

    //8. If you supply all valid values, it should be successful and return an object of BuyOrderResponse type with auto-generated BuyOrderID (guid).
    [Fact]
    public async Task CreateBuyOrder_Valid()
    {
        //Arrange
        BuyOrderRequest buyOrderRequest = new BuyOrderRequest
        {
            DateAndTimeOfOrder = DateTime.Parse("2020-01-01"),
            Price = 10,
            Quantity = 1,
            StockName = "Microsoft",
            StockSymbol = "MSFT"
        };

        //Act
        var response = await _stocksCreateService.CreateBuyOrder(buyOrderRequest);

        //Assert
        Assert.NotEqual(Guid.Empty, response.BuyOrderID);
    }
    #endregion

    #region CreateSellOrder
    //1. When you supply SellOrderRequest as null, it should throw ArgumentNullException.

    //2. When you supply sellOrderQuantity as 0 (as per the specification, minimum is 1), it should throw ArgumentException.
    [Fact]
    public async Task CreateSellOrder_Quantity0()
    {
        //Arrange
        SellOrderRequest? sellOrderRequest = new SellOrderRequest
        {
            DateAndTimeOfOrder = DateTime.Parse("2001-12-31"),
            Price = 10,
            Quantity = 0,
            StockName = "Microsoft",
            StockSymbol = "MSFT"
        };

        //Assert
        await Assert.ThrowsAsync<ArgumentException>(async () =>
            //Act
            await _stocksCreateService.CreateSellOrder(sellOrderRequest));
    }

    //3. When you supply sellOrderQuantity as 100001 (as per the specification, maximum is 100000), it should throw ArgumentException.

    //4. When you supply sellOrderPrice as 0 (as per the specification, minimum is 1), it should throw ArgumentException.

    //5. When you supply sellOrderPrice as 10001 (as per the specification, maximum is 10000), it should throw ArgumentException.

    //6. When you supply stock symbol=null (as per the specification, stock symbol can't be null), it should throw ArgumentException.

    //7. When you supply dateAndTimeOfOrder as "1999-12-31" (YYYY-MM-DD) - (as per the specification, it should be equal or newer date than 2000-01-01), it should throw ArgumentException.

    //8. If you supply all valid values, it should be successful and return an object of SellOrderResponse type with auto-generated SellOrderID(guid).
    [Fact]
    public async Task CreateSellOrder_Valid()
    {
        //Arrange
        SellOrderRequest sellOrderRequest = new SellOrderRequest
        {
            DateAndTimeOfOrder = DateTime.Parse("2020-01-01"),
            Price = 10,
            Quantity = 1,
            StockName = "Microsoft",
            StockSymbol = "MSFT"
        };

        //Act
        var response = await _stocksCreateService.CreateSellOrder(sellOrderRequest);

        //Assert
        Assert.NotEqual(Guid.Empty, response.SellOrderID);
    }
    #endregion

    #region GetAllBuyOrders

    //1. When you invoke this method, by default, the returned list should be empty.
    [Fact]
    public async Task GetBuyOrders_Empty()
    {
        //Arrange

        //Act
        var response = await _stocksGetService.GetBuyOrders();

        //Assert
        Assert.Empty(response);
    }

    //2. When you first add few buy orders using CreateBuyOrder() method; and then invoke GetAllBuyOrders() method; the returned list should contain all the same buy orders.
    [Fact]
    public async Task GetBuyOrders_List()
    {
        //Arrange
        BuyOrderRequest buyOrderRequest1 = new BuyOrderRequest
        {
            DateAndTimeOfOrder = DateTime.Parse("2020-01-01"),
            Price = 10,
            Quantity = 1,
            StockName = "Microsoft",
            StockSymbol = "MSFT"
        };
        BuyOrderRequest buyOrderRequest2 = new BuyOrderRequest
        {
            DateAndTimeOfOrder = DateTime.Parse("2022-01-01"),
            Price = 100,
            Quantity = 2,
            StockName = "Microsoft",
            StockSymbol = "MSFT"
        };
        List<BuyOrderRequest> buyOrderRequests = new() { buyOrderRequest1, buyOrderRequest2 };
        List<BuyOrderResponse> expectedBuyOrders = new();
        foreach (var buyOrderRequest in buyOrderRequests)
        {
            expectedBuyOrders.Add(await _stocksCreateService.CreateBuyOrder(buyOrderRequest));
        }
        //print expected
        _testOutputHelper.WriteLine("Expected");
        foreach (var item in expectedBuyOrders)
        {
            _testOutputHelper.WriteLine(item.ToString());
        }

        //Act
        var actualbuyOrders = await _stocksGetService.GetBuyOrders();
        //print actual
        _testOutputHelper.WriteLine("Actual");
        foreach (var item in actualbuyOrders)
        {
            _testOutputHelper.WriteLine(item.ToString());
        }

        //Assert
        var d1 = actualbuyOrders.Except(expectedBuyOrders);
        var d2 = expectedBuyOrders.Except(actualbuyOrders);

        Assert.Empty(d1);
        Assert.Empty(d2);
    }


    #endregion

    #region GetAllSellOrders
    //1. When you invoke this method, by default, the returned list should be empty.

    [Fact]
    public async Task GetSellOrders_Empty()
    {
        //Arrange

        //Act
        var response = await _stocksGetService.GetSellOrders();

        //Assert
        Assert.Empty(response);
    }

    //2. When you first add few sell orders using CreateSellOrder() method; and then invoke GetAllSellOrders() method; the returned list should contain all the same sell orders.
    [Fact]
    public async Task GetSellOrders_List()
    {
        //Arrange
        SellOrderRequest sellOrderRequest1 = new SellOrderRequest
        {
            DateAndTimeOfOrder = DateTime.Parse("2020-01-01"),
            Price = 10,
            Quantity = 1,
            StockName = "Microsoft",
            StockSymbol = "MSFT"
        };
        SellOrderRequest sellOrderRequest2 = new SellOrderRequest
        {
            DateAndTimeOfOrder = DateTime.Parse("2022-01-01"),
            Price = 100,
            Quantity = 2,
            StockName = "Microsoft",
            StockSymbol = "MSFT"
        };
        List<SellOrderRequest> sellOrderRequests = new() { sellOrderRequest1, sellOrderRequest2 };
        List<SellOrderResponse> expectedSellOrders = new();
        foreach (var sellOrderRequest in sellOrderRequests)
        {
            expectedSellOrders.Add(await _stocksCreateService.CreateSellOrder(sellOrderRequest));
        }
        //print expected
        _testOutputHelper.WriteLine("Expected");
        foreach (var item in expectedSellOrders)
        {
            _testOutputHelper.WriteLine(item.ToString());
        }

        //Act
        var actualSellOrders = await _stocksGetService.GetSellOrders();
        //print actual
        _testOutputHelper.WriteLine("Actual");
        foreach (var item in actualSellOrders)
        {
            _testOutputHelper.WriteLine(item.ToString());
        }

        //Assert
        var d1 = actualSellOrders.Except(expectedSellOrders);
        var d2 = expectedSellOrders.Except(actualSellOrders);

        Assert.Empty(d1);
        Assert.Empty(d2);
    }

    #endregion
}