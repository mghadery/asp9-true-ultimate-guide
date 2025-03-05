using Stocks.Entities;
using Stocks.Helpers.Validators;
using System.ComponentModel.DataAnnotations;

namespace Stocks.ServiceContracts.Interfaces;

public interface OrderRequest
{
    public string? StockSymbol { get; set; }

    public string? StockName { get; set; }
    public DateTime DateAndTimeOfOrder { get; set; }

}
