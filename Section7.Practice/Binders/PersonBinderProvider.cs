using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Section7.Practice.Models;

namespace Section7.Practice.Binders
{
    public class PersonBinderProvider : IModelBinderProvider
    {
        public IModelBinder? GetBinder(ModelBinderProviderContext context)
        {
            if (context.Metadata.ModelType == typeof(Person))
                //return new PersonBinder();
                return new BinderTypeModelBinder(typeof(PersonBinder));
            return null;
        }
    }
}
