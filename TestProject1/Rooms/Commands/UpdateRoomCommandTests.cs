using Application.Abstractions.Repositories;
using Application.Rooms.Commands.UpdateRoom;
using Azure.Core;
using Domain.Entities;
using Domain.Exceptions;
using FluentAssertions;
using NSubstitute;

namespace Application.UnitTests.Rooms.Commands;

public sealed class UpdateRoomCommandTests
{

    private readonly UpdateRoomCommand _command = new (1, null, null, 1);
    private readonly UpdateRoomCommandHandler _handler;
    private readonly IRoomRepository _roomRepositoryMock;
    private readonly IWaiterRepository _waiterRepositoryMock;

    public UpdateRoomCommandTests()
    {
        _roomRepositoryMock = Substitute.For<IRoomRepository>();
        _waiterRepositoryMock = Substitute.For<IWaiterRepository>();
        _handler = new UpdateRoomCommandHandler(_roomRepositoryMock, _waiterRepositoryMock, default!);
    }

    [Fact]
    public async Task Handle_Should_ReturnError_WhenRoomNotFound()
    {
        //Arrange
        _roomRepositoryMock.SearchByIdAsync(Arg.Is<int>(s => s == _command.Id)).Returns((Room?)null);

        //Act
        var result = await _handler.Handle(_command, default);

        //Assert
        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().Contain(e => e.Message == RoomError.NotFound(_command.Id).Message);
    }

    [Fact]
    public async Task Handle_Should_ReturnError_WhenWaiterAssociatedIsNull()
    {
        //Arrange
        _roomRepositoryMock.SearchByIdAsync(Arg.Is<int>(s => s == _command.Id)).Returns(new Room());
        _waiterRepositoryMock.SearchByIdAsync(Arg.Is<int>(id => id == _command.WaiterId)).Returns((Waiter?)null);

        //Act
        var result = await _handler.Handle(_command, default);

        //Assert
        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorType == SharedLib.Models.Common.ApiErrorType.NotFound);
    }

    [Fact]
    public async Task Handle_Should_ReturnError_WhenWaiterAssociatedRoomIdIsNoEqualToRoom()
    {
        //Arrange
        _roomRepositoryMock.SearchByIdAsync(Arg.Is<int>(s => s == _command.Id)).Returns(new Room());
        _waiterRepositoryMock.SearchByIdAsync(Arg.Is<int>(id => id == _command.WaiterId)).Returns<Waiter?>(new Waiter() { Room = new Room() { Id = _command.Id + 1 } });

        //Act
        var result = await _handler.Handle(_command, default);

        //Assert
        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorType == SharedLib.Models.Common.ApiErrorType.Validation);
    }

    [Fact]
    public async Task Handle_Should_ReturnSuccess_WhenRoomExistAndDoesnExistDuplicatedName()
    {

        //Arrange
        _roomRepositoryMock.SearchByIdAsync(Arg.Is<int>(s => s == _command.Id)).Returns(new Room());
        _waiterRepositoryMock.SearchByIdAsync(Arg.Is<int>(id => id == _command.WaiterId)).Returns<Waiter?>(new Waiter() { Room = new Room() { Id = _command.Id } });

        //Act
        var result = await _handler.Handle(_command, default);

        //Assert

        result.IsSuccess.Should().BeTrue();
        result.Errors.Should().BeNull();
    }
}
