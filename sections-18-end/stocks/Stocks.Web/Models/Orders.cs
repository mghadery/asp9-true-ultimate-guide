using Stocks.ServiceContracts.DTOs;

namespace Stocks.Web.Models;

public class Orders
{
    public List<BuyOrderResponse> BuyOrders { get; set; }
    public List<SellOrderResponse> SellOrders { get; set; }
}
