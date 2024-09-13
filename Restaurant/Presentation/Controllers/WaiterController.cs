using Application.Waiters.Commands.CreateWaiter;
using Application.Waiters.Commands.DeleteWaiter;
using Application.Waiters.Commands.UpdateWaiter;
using Application.Waiters.Queries.GetWaiterById;
using Application.Waiters.Queries.GetWaiters;
using Application.Waiters.Queries.GetWaitersByFilter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Presentation.Authentication;
using Presentation.Extensions;

namespace Presentation.Controllers;

public static class WaitersEndPoints 
{
    public static void AddWaitersEndPoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/waiters/").AddEndpointFilter<ApiKeyAuthenticationEndPointFilter>();

        #region Commands

        group.MapPost("createwaiter", CreateAsync);

        group.MapPut("updatewaiter", UpdateAsync);
        
        group.MapDelete("deletewaiter/{id:int}", DeleteAsync);

        #endregion

        #region Queries

        group.MapGet("getwaiterbyid/{id:int}", GetByIdAsync);

        group.MapPost("getwaitersbyfilter", GetByFilterAsync);

        group.MapGet("getwaiters/{responsable:bool}", GetWaitersAsync);

        #endregion
    }

    #region Queries Methods

    public static async Task<IResult> GetByIdAsync(int id, ISender sender, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetWaiterByIdQuery(id), cancellationToken);

        return result.IsSuccess ? Results.Ok(result) : result.ToHttpResponse();
    }

    public static async Task<IResult> GetByFilterAsync([FromBody] GetWaitersByFilterQuery filter, ISender sender, CancellationToken cancellationToken)
    {
        var paged = await sender.Send(filter, cancellationToken);

        return paged.Error is null ? Results.Ok(paged) : paged.ToHttpResponse();
    }

    #endregion


    #region Commands Methods

    public static async Task<IResult> CreateAsync([FromBody] CreateWaiterCommand createWaiterCommand, ISender sender, CancellationToken cancellationToken)
    {
        var result = await sender.Send(createWaiterCommand, cancellationToken);

        return result.IsSuccess ? Results.Ok(result) : result.ToHttpResponse();
    }

    public static async Task<IResult> UpdateAsync([FromBody] UpdateWaiterCommand updateWaiterCommand, ISender sender, CancellationToken cancellationToken)
    {
        var result = await sender.Send(updateWaiterCommand, cancellationToken);

        return result.IsSuccess ? Results.Ok(result) : result.ToHttpResponse();
    }


    public static async Task<IResult> DeleteAsync(int id, ISender sender, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new DeleteWaiterCommand(id), cancellationToken);

        return result.IsSuccess ? Results.Ok(result) : result.ToHttpResponse();
    }

    public static async Task<IResult> GetWaitersAsync(bool responsable, ISender sender, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetWaitersQuery(responsable), cancellationToken);

        return result.IsSuccess ? Results.Ok(result) : result.ToHttpResponse();
    }

    #endregion

}
