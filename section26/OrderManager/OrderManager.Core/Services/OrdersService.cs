using OrderManager.Core.Domain.Entities;
using OrderManager.Core.Domain.RepositoryContracts;
using OrderManager.Core.DTOs;
using OrderManager.Core.ServiceContracts;

namespace OrderManager.Core.Services;

public class OrdersService(IOrderRepository orderRepository) : IOrdersService
{
    public async Task<Order?> GetById(Guid id)
    {
        return await orderRepository.GetById(id);
    }
    public async Task<ListResponse<OrderResponse>> GetAll(string sortBy = "OrderDate", bool asc = true, int pageSize = 0, int pageNo = 1)
    {
        var orders = await orderRepository.Get(x => true, sortBy, asc, pageSize, pageNo);
        var totalCount = await orderRepository.Count(x => true);
        var list = orders.Select(x => new OrderResponse(x)).ToList();
        var count = list.Count();
        ListResponse<OrderResponse> listResponse = new ListResponse<OrderResponse>()
        {
            TotalCount = totalCount,
            Count = count,
            PageNumber = pageNo,
            PageSize = pageSize,
            Result = list
        };
        return listResponse;
    }

    public async Task<Order> Create(string customerName)
    {
        var now = DateTime.Now;
        var thisYear = new DateTime(now.Year, 1, 1);
        var mil = (long)(now - thisYear).TotalMilliseconds;
        Order order = new Order()
        {
            CustomerName = customerName,
            OrderId = Guid.NewGuid(),
            OrderDate = DateTime.Now,
            OrderNumber = $"{now.Year}_{mil}"

        };

        await orderRepository.Create(order);
        return order;
    }

    public async Task<bool> Delete(Guid id)
    {
        return await orderRepository.Delete(id);
    }

    public async Task<List<OrderItem>> GetItemsByOrderId(Guid id)
    {
        return await orderRepository.GetItemsByOrderId(id);
    }

    public async Task<OrderItemResponse> AddItem(OrderItemAddRequest orderItemAddRequest)
    {
        var orderItem = orderItemAddRequest.ToOrderItem();

        //TODO validation and exception handling

        return new OrderItemResponse(await orderRepository.AddItem(orderItem));
    }
    public async Task<OrderItemResponse> UpdateItem(OrderItemUpdateRequest orderItemUpdateRequest)
    {
        var orderItem = orderItemUpdateRequest.ToOrderItem();

        //TODO validation and exception handling

        return new OrderItemResponse(await orderRepository.UpdateItem(orderItem));
    }

    public async Task<bool> DeleteItem(Guid orderId, Guid orderItemId)
    {
        return await orderRepository.DeleteItem(orderId, orderItemId);
    }
}
