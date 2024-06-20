/*using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace CRUDApi.Filter
{
    public class RemoveNullPropertiesResultFilter : ResultFilterAttribute
    {
        public override void OnResultExecuting(ResultExecutingContext context)
        {
            if (context.Result is ObjectResult objectResult)
            {
                var resultObject = objectResult.Value;
                if (resultObject != null)
                {
                    RemoveNullProperties(resultObject);
                }
            }

            base.OnResultExecuting(context);
        }

        public static void RemoveNullProperties(object obj)
        {
            var properties = obj.GetType().GetProperties()
                .Where(property => property.GetValue(obj) == null)
                .ToArray();

            foreach (var property in properties)
            {
                if (property.CanWrite)
                {
                    var propertyType = property.PropertyType;
                    var defaultValue = propertyType.IsValueType ? Activator.CreateInstance(propertyType) : null;
                    property.SetValue(obj, defaultValue);
                }
            }
        }


        private static Action<object, object> CreatePropertySetter(object obj, PropertyInfo property)
        {
            var targetType = property.DeclaringType;
            var parameter = Expression.Parameter(typeof(object), "p");
            var convertedParameter = Expression.Convert(parameter, targetType);

            var propertyAccess = Expression.Property(convertedParameter, property);
            var value = Expression.Parameter(typeof(object));

            var assignment = Expression.Assign(propertyAccess, Expression.Convert(value, property.PropertyType));
            var lambda = Expression.Lambda<Action<object, object>>(assignment, parameter, value);

            return lambda.Compile();
        }
    }
}
*/