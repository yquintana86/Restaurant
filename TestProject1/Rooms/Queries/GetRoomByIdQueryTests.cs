using Application.Abstractions.Repositories;
using Application.Rooms.Queries.GetRoomById;
using Domain.Entities;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using SharedLib.Models.Common;

namespace Application.UnitTests.Rooms.Queries;

public class GetRoomByIdQueryTests
{
    private readonly GetRoomByIdQuery _query = new (1);
    private readonly GetRoomByIdQueryHandler _handler;
    private readonly IRoomRepository _roomRepository;

    public GetRoomByIdQueryTests()
    {
        _roomRepository = Substitute.For<IRoomRepository>();
        _handler = new GetRoomByIdQueryHandler(_roomRepository,default!);
    }

    [Fact]
    public async Task Handle_Should_ReturnError_WhenInvalidQueryId()
    {
        //Arrange
        GetRoomByIdQuery getRoomByIdQuery = new (0);

        //Act
        var result = await _handler.Handle(getRoomByIdQuery, default);

        //Assert
        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorType == ApiErrorType.Validation);
    }

    [Fact]
    public async Task Handle_Should_ReturnError_WhenRoomNotFound()
    {
        //Arrange
        _roomRepository.SearchByIdAsync(Arg.Is<int>(id => id == _query.Id)).ReturnsNull();

        //Act
        var result = await _handler.Handle(_query, default);

        //
        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorType == ApiErrorType.NotFound);
    }

    [Fact]
    public async Task Handle_Should_ReturnSuccess_WhenRoomFoundedHasNullWaiter()
    {
        //Arrange
        _roomRepository.SearchByIdAsync(Arg.Is<int>(id => id == _query.Id)).Returns(new Room() { Waiter = null});

        //Act
        var result = await _handler.Handle(_query, default);

        //Assert
        result.IsSuccess.Should().BeFalse();
        result.Errors!.Should().Contain(e => e.ErrorType == ApiErrorType.Validation);
    }

    [Fact]
    public async Task Handle_Should_ReturnSuccess_WhenRoomFoundedBySearchById()
    {
        //Arrange
        _roomRepository.SearchByIdAsync(Arg.Is<int>(id => id == _query.Id)).Returns(new Room()
        {
            Id = _query.Id,
            Waiter = new Waiter {FirstName = "", LastName="" }
        });

        //Act
        var result = await _handler.Handle(_query, default);

        //Assert
        result.IsSuccess.Should().BeTrue();
        result.Errors.Should().BeNullOrEmpty();
    }





}
