using ContactManager.Core.Domain.Entities;
using ContactManager.Core.Domain.RepositorieContracts;
using ContactManager.Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ContactManager.Infrastructure.Repositories;

public class CountryRepository(AppDbContext dbContext) : ICountryRepository
{
    public CountryRepository() : this(new AppDbContext(new DbContextOptionsBuilder<AppDbContext>().Options))
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
