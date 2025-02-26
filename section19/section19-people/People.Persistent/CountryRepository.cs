using Microsoft.EntityFrameworkCore;
using People.Entities;
using People.ServiceContracts.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace People.Persistent;

public class CountryRepository(AppDbContext dbContext) : ICountryRepository
{
    public CountryRepository():this(new AppDbContext(new DbContextOptionsBuilder<AppDbContext>().Options))
    {
        
    }
    public virtual async Task<Country> Add(Country country)
    {
        await dbContext.Countries.AddAsync(country);
        await dbContext.SaveChangesAsync();
        return country;
    }

    public virtual async Task<List<Country>> GetAll()
    {
        return await dbContext.Countries.ToListAsync();
    }

    public virtual async Task<List<Country>> Get(Expression<Func<Country, bool>> expression)
    {
        return await dbContext.Countries.Where(expression).ToListAsync();
    }

}
