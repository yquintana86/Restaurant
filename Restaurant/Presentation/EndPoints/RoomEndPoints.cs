using Application.Rooms.Commands.CreateRoom;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Mvc;
using Application.Rooms.Commands.UpdateRoom;
using Application.Rooms.Commands.DeleteRoom;
using Application.Rooms.Queries.GetRoomById;
using Application.Rooms.Queries.GetRoomsByFilter;
using Presentation.Extensions;
using Presentation.Authentication;
using Application.Rooms.Queries.GetRooms;

namespace Presentation.Controllers;

public static class RoomEndPoints 
{
    public static void AddRoomEndPoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/rooms/").AddEndpointFilter<ApiKeyAuthenticationEndPointFilter>();

        #region Commands

        group.MapPost("createroom", CreateAsync);

        group.MapPut("updateroom", UpdateAsync);

        group.MapDelete("deleteroom/{id:int}", DeleteAsync);

        #endregion


        #region Queries 

        group.MapGet("getroombyid/{id:int}", GetByIdAsync);

        group.MapPost("searchroomsbyfilter", GetbyFilterAsync);

        group.MapGet("getrooms", GetRoomsAsync);

        #endregion
    }


    #region Commands Methods
    public static async Task<IResult> CreateAsync([FromBody] CreateRoomCommand command, ISender sender, CancellationToken CancellationToken)
    {
        var result = await sender.Send(command, CancellationToken);

        return result.IsSuccess ? Results.Ok(result) : result.ToHttpResponse();
    }

    public static async Task<IResult> UpdateAsync([FromBody] UpdateRoomCommand command, ISender sender, CancellationToken cancellationToken)
    {
        var result = await sender.Send(command, cancellationToken);

        return result.IsSuccess ? Results.Ok(result) : result.ToHttpResponse();
    }

    public static async Task<IResult> DeleteAsync(int id, ISender sender, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new DeleteRoomCommand(id), cancellationToken);

        return result.IsSuccess ? Results.Ok(result) : result.ToHttpResponse();
    }

    #endregion


    #region Queries Methods
    public static async Task<IResult> GetByIdAsync(int id, ISender sender, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetRoomByIdQuery(id), cancellationToken);
        
        return result.IsSuccess ? Results.Ok(result) : result.ToHttpResponse();
    }

    public static async Task<IResult> GetbyFilterAsync([FromBody] GetRoomsByFilterQuery filter, ISender sender, CancellationToken cancellationToken)
    {
        var paged = await sender.Send(filter, cancellationToken);

        return paged.Error is null ? Results.Ok(paged) : paged.ToHttpResponse();
    }

    public static async Task<IResult> GetRoomsAsync(ISender sender, CancellationToken cancellatioToken)
    {
        var result = await sender.Send(new GetRoomsQuery(), cancellatioToken);

        return result.IsSuccess ? Results.Ok(result) : result.ToHttpResponse();
    }

    

    #endregion

}
