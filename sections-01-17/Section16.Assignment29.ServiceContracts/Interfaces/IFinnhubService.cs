namespace Section16.Assignment29.ServiceContracts.Interfaces;

public interface IFinnhubService
{
    Task<Dictionary<string, object>?> GetCompanyProfile(string stockSymbol);
    Task<Dictionary<string, object>?> GetStockPriceQuote(string stockSymbol);
}
