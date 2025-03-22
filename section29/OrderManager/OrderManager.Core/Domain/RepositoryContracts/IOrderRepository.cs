
using System.Linq.Expressions;
using OrderManager.Core.Domain.Entities;

namespace OrderManager.Core.Domain.RepositoryContracts;

public interface IOrderRepository
{
    Task<Order> Create(Order order);
    Task<Order> Update(Order order);
    Task<bool> Delete(Guid id);
    Task<Order?> GetById(Guid id);
    Task<List<Order>> Get(Expression<Func<Order, bool>> expression, string sortBy, bool asc, int pageSize, int pageNo);
    Task<int> Count(Expression<Func<Order, bool>> expression);
    Task<List<OrderItem>> GetItemsByOrderId(Guid id);
    Task<OrderItem> AddItem(OrderItem item);
    Task<OrderItem> UpdateItem(OrderItem item);
    Task<bool> DeleteItem(Guid orderId, Guid orderItemId);
}
