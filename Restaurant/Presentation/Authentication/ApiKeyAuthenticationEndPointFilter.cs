using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Presentation.Authentication;

internal sealed class ApiKeyAuthenticationEndPointFilter : IEndpointFilter
{
    private readonly string ApiKeyHeaderName;
    private readonly IConfiguration _configuration;

    public ApiKeyAuthenticationEndPointFilter(IConfiguration configuration)
    {
        _configuration = configuration;
        ApiKeyHeaderName = _configuration.GetSection("ApiSettings")["Header"] ?? "";

        if(string.IsNullOrWhiteSpace(ApiKeyHeaderName))
             ArgumentException.ThrowIfNullOrWhiteSpace(nameof(ApiKeyHeaderName));
    }

    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {

        string? key = context.HttpContext.Request.Headers[ApiKeyHeaderName];

        if (!IsKeyValid(key))
        {
            return Results.Unauthorized();
        }

        return await next(context);
    }


    private bool IsKeyValid(string? key)
    {
        if (string.IsNullOrEmpty(key))
            return false;

        string keyStored = _configuration.GetSection("ApiSettings")["ApiKey"] ?? ""; 

        return key == keyStored;
    }


}
