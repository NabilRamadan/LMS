using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

public class DataValidationMiddleware
{
    private readonly RequestDelegate _next;
    private readonly List<EndpointValidationConfig> _validationConfigs;

    public DataValidationMiddleware(RequestDelegate next, List<EndpointValidationConfig> validationConfigs)
    {
        _next = next;
        _validationConfigs = validationConfigs;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        foreach (var config in _validationConfigs)
        {
            if (context.Request.Path.StartsWithSegments(config.EndpointPath) && context.Request.Method == config.HttpMethod)
            {
                if (!context.Request.Query.ContainsKey(config.RequiredParameter))
                {
                    context.Response.StatusCode = 400;
                    await context.Response.WriteAsync($"Parameter '{config.RequiredParameter}' is required for this endpoint.");
                    return;
                }


            }
        }

        await _next(context);
    }
}
