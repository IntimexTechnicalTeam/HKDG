using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;
using System;
using System.Linq;
using Web.Framework;

namespace Web.Swagger
{
    /// <summary>
    /// 隐藏Api特性
    /// </summary>
    public class HiddenApiFilter : IDocumentFilter
    {
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            foreach (var item in context.ApiDescriptions)
            {
                if (item.TryGetMethodInfo(out MethodInfo methodInfo))
                {
                    if (methodInfo.ReflectedType.CustomAttributes.Any(t => t.AttributeType == typeof(HiddenAttribute))
                    || methodInfo.CustomAttributes.Any(t => t.AttributeType == typeof(HiddenAttribute)))
                    {
                        var key = "/" + item.RelativePath.TrimEnd('/');
                        if (key.Contains("?"))
                        {
                            int idx = key.IndexOf("?", StringComparison.Ordinal);
                            key = key.Substring(0, idx);
                        }
                        if (swaggerDoc.Paths.ContainsKey(key))
                        {
                            swaggerDoc.Paths.Remove(key);
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    ///  隐藏字段特性
    /// </summary>
    public class HiddenFieldFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            if (schema?.Properties == null)
            {
                return;
            }
            var name = context.Type.FullName;
            var excludedProperties = context.Type.GetProperties();
            foreach (var property in excludedProperties)
            {
                var attribute = property.GetCustomAttribute<HiddenFieldAttribute>();
                if (attribute != null
                    && schema.Properties.ContainsKey(ToLowerStart(property.Name)))
                {
                    schema.Properties.Remove(ToLowerStart(property.Name));
                }
            };
        }
        public static string ToLowerStart(string source)
        {
            if (string.IsNullOrWhiteSpace(source))
            {
                return source;
            }
            var start = source.Substring(0, 1);
            return $"{start.ToLower()}{source.Substring(1, source.Length - 1)}";
        }
    }
}
