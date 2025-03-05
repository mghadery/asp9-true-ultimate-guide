namespace Stocks.ServiceContracts.Interfaces;

public interface IFinnhubService
{
    Task<List<Dictionary<string, string>>> GetStocks();
    Task<Dictionary<string, object>?> GetCompanyProfile(string stockSymbol);
    Task<Dictionary<string, object>?> GetStockPriceQuote(string stockSymbol);
}
