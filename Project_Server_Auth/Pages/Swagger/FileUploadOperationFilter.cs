namespace pr_srv_names.Supprts.Swagger
{
    // Фильтр для Swagger (если используете)
    public class FileUploadOperationFilter : Swashbuckle.AspNetCore.SwaggerGen.IOperationFilter
    {
        public void Apply(Microsoft.OpenApi.Models.OpenApiOperation operation, Swashbuckle.AspNetCore.SwaggerGen.OperationFilterContext context)
        {
            var hasFileParameter = context.MethodInfo.GetParameters()
                .Any(p => p.ParameterType == typeof(IFormFile) ||
                          p.ParameterType == typeof(IEnumerable<IFormFile>) ||
                          p.ParameterType == typeof(List<IFormFile>));

            if (!hasFileParameter) return;

            operation.RequestBody = new Microsoft.OpenApi.Models.OpenApiRequestBody
            {
                Content = new Dictionary<string, Microsoft.OpenApi.Models.OpenApiMediaType>
                {
                    ["multipart/form-data"] = new Microsoft.OpenApi.Models.OpenApiMediaType
                    {
                        Schema = new Microsoft.OpenApi.Models.OpenApiSchema
                        {
                            Type = "object",
                            Properties = new Dictionary<string, Microsoft.OpenApi.Models.OpenApiSchema>
                            {
                                ["file"] = new Microsoft.OpenApi.Models.OpenApiSchema
                                {
                                    Type = "string",
                                    Format = "binary"
                                }
                            }
                        }
                    }
                }
            };
        }
    }
}
