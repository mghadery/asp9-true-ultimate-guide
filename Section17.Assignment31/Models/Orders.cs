using Section16.Assignment29.ServiceContracts.DTOs;

namespace Section17.Assignment31.Web.Models;

public class Orders
{
    public List<BuyOrderResponse> BuyOrders { get; set; }
    public List<SellOrderResponse> SellOrders { get; set; }
}
