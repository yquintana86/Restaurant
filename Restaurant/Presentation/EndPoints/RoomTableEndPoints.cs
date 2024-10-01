using Application.RoomTable.Commands.CreateTable;
using Application.RoomTable.Commands.DeleteTable;
using Application.RoomTable.Commands.UpdateTable;
using Application.RoomTable.Queries.GetTablesbyFilter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Presentation.Authentication;
using Presentation.Extensions;

namespace Presentation.Controllers;

public static class RoomTableEndpoints
{
    public static void AddRoomTableEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/roomtables/").AddEndpointFilter<ApiKeyAuthenticationEndPointFilter>();


        #region Commands

        group.MapPost("createroomtable", CreateAsync);

        group.MapPut("updateroomtable", UpdateAsync);

        group.MapDelete("{id:int}/{roomId:int}", DeleteAsync);

        #endregion

        #region Queries

        group.MapPost("getroomtablesbyfilter", SearchByFilterAsync);

        #endregion
    }

    #region Commands Methods

    public static async Task<IResult> CreateAsync([FromBody] CreateRoomTableCommand roomTableCommand, ISender sender, CancellationToken cancellationToken)
    {        
        var result = await sender.Send(roomTableCommand, cancellationToken);

        return result.IsSuccess ? Results.Ok(result) : result.ToHttpResponse();
    }

    public static async Task<IResult> UpdateAsync([FromBody] UpdateRoomTableCommand roomTableCommand, ISender sender, CancellationToken cancellationToken)
    {
        var result = await sender.Send(roomTableCommand, cancellationToken);

        return result.IsSuccess ? Results.Ok(result) : result.ToHttpResponse();
    }

    public static async Task<IResult> DeleteAsync(int id, int roomId, ISender sender, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new DeleteRoomTableCommand(id, roomId), cancellationToken);

        return result.IsSuccess ? Results.Ok(result) : result.ToHttpResponse();
    }

    #endregion

    #region Queries Methods

    public static async Task<IResult> SearchByFilterAsync([FromBody] GetRoomTablesByFilterQuery tableQuery, ISender sender, CancellationToken cancellationToken)
    {
        var paged = await sender.Send(tableQuery,cancellationToken);

        return paged.Error is null ? Results.Ok(paged) : paged.ToHttpResponse();
    }

    #endregion
}
