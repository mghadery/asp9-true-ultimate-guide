using Microsoft.EntityFrameworkCore;
using OrderManager.Core.Domain.Entities;
using OrderManager.Core.Domain.RepositoryContracts;
using OrderManager.Infrastructure.Persistent.DbContexts;
using System.Linq.Expressions;

namespace OrderManager.Infrastructure.Persistent.Repositories;

public class OrderRepository(AppDbContext dbContext): IOrderRepository
{
    public async Task<Order> Create(Order order)
    {
        dbContext.Orders.Add(order);
        await dbContext.SaveChangesAsync();
        return order;
    }
    public async Task<Order> Update(Order order)
    {
        var dbOrder = await dbContext.Orders.FindAsync(order.OrderId);
        if (dbOrder is null)
            throw new ArgumentException($"Order with id {order.OrderId} does not exist");

        dbContext.Orders.Attach(order);
        dbContext.Entry(order).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
        await dbContext.SaveChangesAsync();
        return order;
    }
    public async Task<bool> Delete(Guid id)
    {
        var dbOrder = await dbContext.Orders.Include(x => x.OrderItems)
            .Where(x => x.OrderId == id).FirstOrDefaultAsync();
        if (dbOrder is null)
            return false;

        using (var tr = dbContext.Database.BeginTransaction()) //Better to be in the service layer using UOW or alternatively using cascade on delete
        {
            try
            {
                dbContext.Orders.Remove(dbOrder);
                if (dbOrder.OrderItems is not null)
                    dbContext.OrderItems.RemoveRange(dbOrder.OrderItems);
                await dbContext.SaveChangesAsync();
                await tr.CommitAsync();
            }
            catch (Exception exp)
            {
                await tr.RollbackAsync();
                throw;
            }
        }
        return true;
    }
    public async Task<Order?> GetById(Guid id)
    {
        var dbOrder = await dbContext.Orders.FindAsync(id);
        return dbOrder;
    }
    public async Task<List<Order>> Get(Expression<Func<Order, bool>> expression, string sortBy, bool asc, int pageSize, int pageNo)
    {
        var queryable = dbContext.Orders.Where(expression);
        string sortMethod = asc ? "OrderBy" : "OrderByDescending";
        
        var parameter = Expression.Parameter(typeof(Order), "x");
        var property = Expression.Property(parameter, sortBy);
        var lambda = Expression.Lambda(property, parameter);
        var resultExpression = Expression.Call(
            typeof(Queryable),
            sortMethod,
            [typeof(Order), property.Type],
            queryable.Expression,
            Expression.Quote(lambda)
            );
        queryable = queryable.Provider.CreateQuery<Order>(resultExpression);
        if (pageSize > 0)
            queryable = queryable.Skip(pageNo * pageSize).Take(pageSize);
        var result = await queryable.ToListAsync();
        return result;
    }
    public async Task<int> Count(Expression<Func<Order, bool>> expression)
    {
        var count = await dbContext.Orders.CountAsync(expression);
        return count;
    }
    public async Task<List<OrderItem>> GetItemsByOrderId(Guid id)
    {
        var items = await dbContext.OrderItems
            .Where(x => x.OrderId == id)
            .ToListAsync();
        return items;
    }
    public async Task<OrderItem> AddItem(OrderItem item)
    {
        var dbOrder = await dbContext.Orders.Include(x => x.OrderItems)
            .Where(x => x.OrderId == item.OrderId).FirstOrDefaultAsync();
        if (dbOrder is null)
            throw new ArgumentException("Invalid order id");
        if (dbOrder.OrderItems is null)
            dbOrder.OrderItems = new List<OrderItem>();
        dbOrder.OrderItems.Add(item);
        dbContext.Entry(item).State = EntityState.Added; //TODO
        //dbContext.OrderItems.Add(item);
        dbOrder.TotalAmount = dbOrder.OrderItems.Sum(x => x.TotalPrice);
        await dbContext.SaveChangesAsync();
        return item;
    }
    public async Task<OrderItem> UpdateItem(OrderItem item)
    {        
        var dbOrder = await dbContext.Orders.Include(x => x.OrderItems)
            .FirstOrDefaultAsync(x => x.OrderId == item.OrderId);
        if (dbOrder is null)
            throw new ArgumentException("Invalid order id");
        var dbOrderItem = dbOrder.OrderItems?.FirstOrDefault(x => x.OrderItemId == item.OrderItemId);
        if (dbOrderItem is null)
            throw new KeyNotFoundException("Invalid order item id");

        dbOrderItem.UnitPrice = item.UnitPrice;
        dbOrderItem.Quantity = item.Quantity;
        dbOrderItem.TotalPrice = item.TotalPrice;        
        
        dbOrder.TotalAmount = dbOrder.OrderItems.Sum(x => x.TotalPrice);
        await dbContext.SaveChangesAsync();
        return item;
    }

    public async Task<bool> DeleteItem(Guid orderId, Guid orderItemId)
    {
        var dbOrder = await dbContext.Orders.Include(x => x.OrderItems)
           .FirstOrDefaultAsync(x => x.OrderId == orderId);
        if (dbOrder is null)
            return false;
        var dbOrderItem = dbOrder.OrderItems?.FirstOrDefault(x => x.OrderItemId == orderItemId);
        if (dbOrderItem is null)
            return false;

        dbOrder.OrderItems?.Remove(dbOrderItem);

        dbOrder.TotalAmount = dbOrder.OrderItems.Sum(x => x.TotalPrice);
        await dbContext.SaveChangesAsync();
        return true;
    }
}
