using Application.Shifts.Commands.CreateShift;
using Application.Shifts.Commands.DeleteShift;
using Application.Shifts.Commands.UpdateShift;
using Application.Shifts.Queries.GetShiftById;
using Application.Shifts.Queries.GetShiftByLookup;
using Application.Shifts.Queries.GetShiftsByFilter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Presentation.Authentication;
using Presentation.Extensions;

namespace Presentation.Controllers;

public static class ShiftEndPoints
{
    public static void AddShiftEndPoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/shift/").AddEndpointFilter<ApiKeyAuthenticationEndPointFilter>();


        #region Commands

        group.MapPost("createshift", CreateAsync);

        group.MapPut("updateshift", UpdateAsync);

        group.MapDelete("{id:int}", DeleteAsync);

        #endregion

        #region Queries

        group.MapGet("getallshifts", GetAllShiftAsync);

        group.MapGet("{id:int}", GetShiftAsync);

        group.MapPost("searchshiftbyfilter", SearchByFilterAsync);

        #endregion
    }

    #region Commands Methods

    public static async Task<IResult> CreateAsync([FromBody] CreateShiftCommand shiftCommand, ISender sender, CancellationToken cancellationToken)
    {
        
        var result = await sender.Send(shiftCommand, cancellationToken);

        return result.IsSuccess ? Results.Ok(result) : result.ToHttpResponse();

    }

    public static async Task<IResult> UpdateAsync([FromBody] UpdateShiftCommand shiftCommand, ISender sender, CancellationToken cancellationToken)
    {
        var result = await sender.Send(shiftCommand, cancellationToken);

        return result.IsSuccess ? Results.Ok(result) : result.ToHttpResponse(); 
    }

    public static async Task<IResult> DeleteAsync(int id, ISender sender, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new DeleteShiftCommand(id), cancellationToken);

        return result.IsSuccess ? Results.Ok(result) : result.ToHttpResponse();
    }

    #endregion

    #region Queries Methods
    public static async Task<IResult> GetShiftAsync(int id, ISender sender, CancellationToken cancellationToken = default)
    {
        var result = await sender.Send(new GetShiftByIdQuery(id), cancellationToken);

        var a = result.IsSuccess ? Results.Ok(result) : result.ToHttpResponse();

        return a;
    }

    public static async Task<IResult> SearchByFilterAsync([FromBody] GetShiftsByFilterQuery getShiftsByFilterQuery, ISender sender, CancellationToken cancellationToken)
    {
        var paged = await sender.Send(getShiftsByFilterQuery,cancellationToken);

        return paged.Error is null ? Results.Ok(paged) : paged.ToHttpResponse();
    }

    public static async Task<IResult> GetAllShiftAsync(ISender sender, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetShiftsQuery(), cancellationToken);

        return result.IsSuccess ? Results.Ok(result) : result.ToHttpResponse();
    }

    #endregion
}
