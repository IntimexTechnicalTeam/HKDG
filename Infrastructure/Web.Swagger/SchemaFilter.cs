﻿using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.Swagger
{
    public class SchemaFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            if (!context.Type.IsClass || context.Type == typeof(string) || !context.Type.IsPublic || context.Type.IsArray) return;
            var obj = Activator.CreateInstance(context.Type);
            _ = (from sc in schema.Properties
                 join co in context.Type.GetProperties() on sc.Key.ToLower() equals co.Name.ToLower()
                 select sc.Value.Example = co.GetValue(obj) != null ? OpenApiAnyFactory.CreateFor(sc.Value, co.GetValue(obj)) : sc.Value.Example).ToList();
        }
    }
}
