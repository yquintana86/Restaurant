using Application.Abstractions.Repositories;
using Application.Rooms.Queries.GetRoomsByFilter;
using Domain.Entities;
using Domain.Exceptions;
using FluentAssertions;
using NSubstitute;
using SharedLib.Models.Common;

namespace Application.UnitTests.Rooms.Queries;

public class GetRoomsbyFilterTests
{
    private readonly GetRoomsByFilterQueryHandler _handler;
    private readonly IRoomRepository _roomRepository;
    private readonly GetRoomsByFilterQuery _query = new();

    public GetRoomsbyFilterTests()
    {
        _roomRepository = Substitute.For<IRoomRepository>();
        _handler = new GetRoomsByFilterQueryHandler(_roomRepository, default!);
    }

    [Fact]
    public async Task Handle_Should_ReturnError_WhenInvalidWorkerIdRequest()
    {
        //Arrange
        GetRoomsByFilterQuery getRoomsByFilterQuery = new() { WaiterId = 0 };

        //Act
        var result = await _handler.Handle(getRoomsByFilterQuery, default!);

        //Assert
        result.Error.Should().NotBeNull();
        result.Error!.ErrorType.Should().Be(ApiErrorType.Validation);
    }

    [Fact]
    public async Task Handle_Should_ReturnSucces_WhenValidFilter()
    {
        //Arrange
        _roomRepository.GetbyFilterAsync(Arg.Is<GetRoomsByFilterQuery>(f => f.Name == _query.Name &&
                                                                            f.WaiterId == _query.WaiterId)).Returns(new PagedResult<Room>());
        //Act
        var result = await _handler.Handle(_query, default!);

        //Assert
        result.Error.Should().BeNull();
    }

}
