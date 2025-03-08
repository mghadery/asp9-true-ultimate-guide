using ContactManager.Core.Domain.Entities;
using ContactManager.Core.Domain.RepositorieContracts;
using ContactManager.Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ContactManager.Infrastructure.Repositories;

public class PersonRepository(AppDbContext dbContext) : IPersonRepository
{
    public PersonRepository() : this(new AppDbContext(new DbContextOptionsBuilder<AppDbContext>().Options))
    {

    }
    public virtual async Task<Person> Add(Person person)
    {
        await dbContext.Persons.AddAsync(person);
        await dbContext.SaveChangesAsync();
        await dbContext.Entry(person).Reference(p => p.Country).LoadAsync();
        return person;
    }

    public virtual async Task<bool> Delete(Person person)
    {
        dbContext.Persons.Remove(person);
        int r = await dbContext.SaveChangesAsync();
        return r > 0;
    }
    public virtual async Task<bool> DeleteById(Guid id)
    {
        var persons = await dbContext.Persons.Where(p => p.PersonId == id).ToListAsync();
        dbContext.Persons.RemoveRange(persons);
        int r = await dbContext.SaveChangesAsync();
        return r > 0;
    }

    public virtual async Task<List<Person>> Get(Expression<Func<Person, bool>> expression)
    {
        var persons = await dbContext.Persons.Include(p => p.Country).Where(expression).ToListAsync();
        return persons;
    }

    public virtual async Task<List<Person>> GetAll()
    {
        return await dbContext.Persons.Include(p => p.Country).ToListAsync();
    }

    public virtual async Task<Person?> GetById(Guid id)
    {
        return (await dbContext.Persons.Include(p => p.Country)
            .Where(p => p.PersonId == id).ToListAsync())
            .FirstOrDefault();
    }

    public virtual async Task<Person> Update(Person person)
    {
        //dbContext.Persons.Update(person);

        dbContext.Attach(person);
        dbContext.Entry(person).State = EntityState.Modified;

        await dbContext.SaveChangesAsync();
        await dbContext.Entry(person).Reference(p => p.Country).LoadAsync();
        return person;
    }
}
