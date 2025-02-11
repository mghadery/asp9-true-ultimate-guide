namespace Section14.Assignment25.ServiceContracts;

public interface IFinnhubService
{
    Task<Dictionary<string, object>?> GetCompanyProfile(string stockSymbol);
    Task<Dictionary<string, object>?> GetStockPriceQuote(string stockSymbol);
}
