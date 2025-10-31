using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Globalization;

namespace Punto_de_ventaMVC.ModelBinders
{
    // 1. Proveedor que verifica si el tipo es 'decimal' o 'decimal?'
    public class InvariantDecimalModelBinderProvider : IModelBinderProvider
    {
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            if (context.Metadata.ModelType == typeof(decimal) ||
                context.Metadata.ModelType == typeof(decimal?))
            {
                // Si es decimal, usa nuestro ModelBinder personalizado
                return new InvariantDecimalModelBinder();
            }

            return null;
        }
    }

    // 2. Model Binder que realiza la conversión usando CultureInfo.InvariantCulture
    public class InvariantDecimalModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null) throw new ArgumentNullException(nameof(bindingContext));

            var valueProviderResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);

            if (valueProviderResult == ValueProviderResult.None)
            {
                return Task.CompletedTask;
            }

            bindingContext.ModelState.SetModelValue(bindingContext.ModelName, valueProviderResult);

            var value = valueProviderResult.FirstValue;

            // La clave es CultureInfo.InvariantCulture, que SIEMPRE usa el punto (.) como decimal.
            if (!decimal.TryParse(value, NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands, CultureInfo.InvariantCulture, out decimal result))
            {
                // Solo agrega el error si el campo es requerido y la conversión falla.
                if (bindingContext.ModelMetadata.IsRequired)
                {
                    bindingContext.ModelState.TryAddModelError(bindingContext.ModelName, "El valor no es un número decimal válido.");
                }

                return Task.CompletedTask;
            }

            bindingContext.Result = ModelBindingResult.Success(result);
            return Task.CompletedTask;
        }
    }
}
