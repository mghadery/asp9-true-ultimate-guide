using Microsoft.EntityFrameworkCore;
using Stocks.Core.Domain.RepositoryContracts;
using Stocks.Infrastructure.DbContexts;
using Stocks.ServiceContracts.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stocks.Infrastructure.Repositories;

public class StocksRepo<T> : IStocksRepo<T> where T : class
{
    private readonly StocksDbContext _dbContext;
    private readonly DbSet<T> _dbSet;

    public StocksRepo(StocksDbContext dbContext)
    {
        _dbContext = dbContext;
        _dbSet = _dbContext.Set<T>();
    }

    public async Task<int> Add(T entity)
    {
        _dbSet.Add(entity);
        int r = await _dbContext.SaveChangesAsync();
        return r;
    }

    public async Task<IEnumerable<T>> GetAll()
    {
        var r = await _dbSet.ToListAsync();
        return r;
    }

    public async Task<T> GetById(Guid guid)
    {
        var r = await _dbSet.FindAsync(guid);
        return r;
    }

    public async Task<int> Remove(T entity)
    {
        _dbSet.Remove(entity);
        int r = await _dbContext.SaveChangesAsync();
        return r;
    }
}
