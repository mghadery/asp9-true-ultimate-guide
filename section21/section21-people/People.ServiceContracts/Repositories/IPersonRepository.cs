using People.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace People.ServiceContracts.Repositories;

public interface IPersonRepository
{
    Task<Person> Add(Person person);
    Task<Person> Update(Person person);
    Task<List<Person>> GetAll();
    Task<Person?> GetById(Guid id);
    Task<List<Person>> Get(Expression<Func<Person, bool>> expression);
    Task<bool> Delete(Person person);
    Task<bool> DeleteById(Guid id);

}
