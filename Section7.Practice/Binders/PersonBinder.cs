using Microsoft.AspNetCore.Mvc.ModelBinding;
using Section7.Practice.Models;

namespace Section7.Practice.Binders
{
    public class PersonBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            Person person = new();
            var name = bindingContext.ValueProvider.GetValue("Name").FirstValue;

            person.Name = "Mr/Ms " + name;
            bindingContext.Result = ModelBindingResult.Success(person);

            return Task.CompletedTask; 

        }
    }
}
