using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Section07.Practice.Models;

namespace Section07.Practice.Binders
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
