using Stocks.ServiceContracts.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stocks.ServiceContracts.Interfaces;

public interface IStocksRepo<T>
{
    Task<int> Add(T entity);

    Task<int> Remove(T entity);

    Task<IEnumerable<T>> GetAll();

    Task<T> GetById(Guid guid);
}
