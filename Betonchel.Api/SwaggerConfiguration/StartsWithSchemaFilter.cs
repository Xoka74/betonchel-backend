using System.Reflection;
using Betonchel.Domain.CustomAttributes;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Betonchel.Api.SwaggerConfiguration;

public class StartsWithSchemaFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (context.MemberInfo?.GetCustomAttributes(typeof(StringStartsWithAttribute), true).Length > 0)
        {
            var attribute = context.MemberInfo.GetCustomAttribute<StringStartsWithAttribute>();
            schema.Description += $"startsWith: {attribute?.Prefix}";
        }
    }
}