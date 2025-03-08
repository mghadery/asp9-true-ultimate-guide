using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stocks.Core.DTOs;

public interface IOrderResponse
{
    string? StockSymbol { get; set; }
    string? StockName { get; set; }
    DateTime DateAndTimeOfOrder { get; set; }
    uint Quantity { get; set; }
    double Price { get; set; }
    double TradeAmount { get; set; }
    string OrderType { get; }
}
