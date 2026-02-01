using System.ComponentModel;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace JobSearchSiteBackend.API.ModelBinders;

public class CommaSeparatedArrayModelBinder : IModelBinder
{
    public Task BindModelAsync(ModelBindingContext bindingContext)
    {
        var valueProviderResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);

        if (valueProviderResult == ValueProviderResult.None) return Task.CompletedTask;

        var value = valueProviderResult.FirstValue;

        if (string.IsNullOrEmpty(value)) return Task.CompletedTask;

        // Split the string by comma
        var elementType = bindingContext.ModelType.GetElementType() ?? bindingContext.ModelType.GetGenericArguments()[0];
        var converter = TypeDescriptor.GetConverter(elementType);

        var values = value.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                          .Select(x => converter.ConvertFromString(x.Trim()))
                          .ToArray();

        // Create a typed array/list to return
        var typedValues = Array.CreateInstance(elementType, values.Length);
        values.CopyTo(typedValues, 0);

        bindingContext.Result = ModelBindingResult.Success(typedValues);
        return Task.CompletedTask;
    }
}