using OrderManager.Core.Domain.Entities;
using OrderManager.Core.Domain.RepositoryContracts;
using OrderManager.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManager.Core.ServiceContracts;

public interface IOrdersService
{
    Task<Order?> GetById(Guid id);
    Task<ListResponse<OrderResponse>> GetAll(string sortBy = "OrderDate", bool asc = true, int pageSize = 0, int pageNo = 1);
    Task<Order> Create(string customerName);
    Task<bool> Delete(Guid id);
    Task<List<OrderItem>> GetItemsByOrderId(Guid id);
    Task<OrderItemResponse> AddItem(OrderItemAddRequest orderItemAddRequest);
    Task<OrderItemResponse> UpdateItem(OrderItemUpdateRequest orderItemUpdateRequest);
    Task<bool> DeleteItem(Guid orderId, Guid orderItemId);
}
