using People.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace People.ServiceContracts.Repositories;

public interface ICountryRepository
{
    Task<List<Country>> GetAll();
    Task<Country> Add(Country country);
    Task<List<Country>> Get(Expression<Func<Country, bool>> expression);
}
