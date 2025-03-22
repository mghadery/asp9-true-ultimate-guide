using OrderManager.Core.Domain.Entities;

namespace OrderManager.Core.DTOs;

public class OrderResponse
{
    public Guid OrderId { get; set; }
    public string? OrderNumber { get; set; }
    public string? CustomerName { get; set; }
    public DateTime? OrderDate { get; set; }
    public decimal TotalAmount { get; set; }

    public OrderResponse(Order order)
    {
        OrderId = order.OrderId;
        OrderNumber = order.OrderNumber;
        CustomerName = order.CustomerName;
        OrderDate = order.OrderDate;
        TotalAmount = order.TotalAmount;
    }
}
