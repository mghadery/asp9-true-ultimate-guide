namespace Stocks.Core.DTOs;

public class TradingOptions
{
    public string? DefaultStockSymbol { get; set; }
    public uint DefaultOrderQuantity { get; set; }
    public string? Top25PopularStocks { get; set; }
}
