using Microsoft.AspNetCore.Http;
using SharedLib.Models.Common;

namespace Presentation.Extensions;

internal static class ResultExtensions
{
    public static IResult ToHttpResponse(this ApiOperationResult result)
    {
        if (result.IsSuccess || result.Errors is null || !result.Errors.Any() )
            ArgumentOutOfRangeException.ThrowIfNotEqual(result.IsSuccess,false);

        if (result.Errors is null || !result.Errors.Any())
            ArgumentException.ThrowIfNullOrEmpty(nameof(result.Errors));

        return GetIResultByResultType(result.Errors!.First().ErrorType, result);
    }

    public static IResult ToHttpResponse<T>(this PagedResult<T> paged)
    {
        if (paged.Error is null)
            ArgumentException.ThrowIfNullOrEmpty(nameof(paged.Error));

        return GetIResultByResultType(paged.Error!.ErrorType, paged);
    }

    private static IResult GetIResultByResultType<T>(ApiErrorType apiErrorType, T result)
    {
        switch (apiErrorType)
        {
            case ApiErrorType.Conflict:
                return Results.Conflict(result);
            case ApiErrorType.NotFound:
                return Results.NotFound(result);

            default: return Results.BadRequest(result);
        }
    }
}
