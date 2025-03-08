using Stocks.Core.DTOs;

namespace Stocks.Core.FinnhubServiceContracts;

public interface IFinnhubService
{
    Task<List<Dictionary<string, string>>> GetStocks();
    Task<List<Stock>> GetDefaultStocksSorted(string[] defaultStocks);
    Task<Dictionary<string, object>?> GetCompanyProfile(string stockSymbol);
    Task<Dictionary<string, object>?> GetStockPriceQuote(string stockSymbol);
}
