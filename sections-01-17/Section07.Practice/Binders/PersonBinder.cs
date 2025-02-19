using Microsoft.AspNetCore.Mvc.ModelBinding;
using Section07.Practice.Models;

namespace Section07.Practice.Binders
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
