﻿using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace ApiApplication.Versioning
{
    public class ReplaceVersionFilter : IDocumentFilter
    {
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            var paths = swaggerDoc.Paths;

            swaggerDoc.Paths = new OpenApiPaths();

            foreach (var path in paths)
            {
                var key = path.Key.Replace("v{version}", swaggerDoc.Info.Version);

                var value = path.Value;

                swaggerDoc.Paths.Add(key, value);
            }
        }
    }
}
