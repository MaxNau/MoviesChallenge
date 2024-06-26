﻿using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Linq;

namespace ApiApplication.Versioning
{
    public class VersionHeaderFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.Parameters != null)
            {
                var versionParameter = operation.Parameters.SingleOrDefault(p => p.Name == "version");

                if (versionParameter != null)
                    operation.Parameters.Remove(versionParameter);
            }
        }
    }
}
