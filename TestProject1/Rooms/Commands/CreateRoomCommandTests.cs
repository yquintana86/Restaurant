using Application.Abstractions.Repositories;
using Application.Rooms.Commands.CreateRoom;
using Domain.Entities;
using Domain.Exceptions;
using FluentAssertions;
using NSubstitute;

namespace Application.UnitTests.Rooms.Commands;

public sealed class CreateRoomCommandTests
{
    private readonly CreateRoomCommand _command = new ("Italian",null,null, 1);
    private readonly CreateRoomCommandHandler _handler;
    private readonly IRoomRepository _roomRepositoryMock;
    private readonly IWaiterRepository _waiterRepositoryMock;


    public CreateRoomCommandTests()
    {
        _roomRepositoryMock = Substitute.For<IRoomRepository>();
        _waiterRepositoryMock = Substitute.For<IWaiterRepository>();
        _handler = new (_roomRepositoryMock, _waiterRepositoryMock, default); 
    }


    [Fact]
    public async Task Handle_Should_ReturnError_WhenRoomExistByName()
    {
        //Arrange
        _roomRepositoryMock.ExistNameAsync(Arg.Is<string>(s => s == _command.Name)).Returns(true);

        //Act
        var result = await _handler.Handle(_command,default);

        //Assert
        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().Contain(e => e.Message == RoomError.RoomNameDuplicated(_command.Name).Message);
    }

    [Fact]
    public async Task Handle_Should_ReturnError_WhenRelatedWaiterNotFound()
    {
        //Arrange
        _roomRepositoryMock.ExistNameAsync(Arg.Is<string>(n => n == _command.Name)).Returns(false);

        _waiterRepositoryMock.SearchByIdAsync(Arg.Is<int>(id => id == _command.WaiterId),default).Returns((Waiter?)null);

        //Act
        var result = await _handler.Handle(_command, default);

        //Assert
        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorType == SharedLib.Models.Common.ApiErrorType.NotFound);
    }

    [Fact]
    public async Task Handle_Should_ReturnError_WhenWaiterHasRoomAssociated()
    {
        //Arrange
        _roomRepositoryMock.ExistNameAsync(Arg.Is<string>(n => n == _command.Name)).Returns(false);
        _waiterRepositoryMock.SearchByIdAsync(Arg.Is<int>(id => id == _command.WaiterId)).Returns<Waiter?>(new Waiter() { Room = new Room() { Id = 1 } });

        //Act
        var result = await _handler.Handle(_command, default);

        //Assert
        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorType == SharedLib.Models.Common.ApiErrorType.Validation);
    }

    [Fact]
    public async Task Handle_Should_ReturnSuccess_WhenRoomNameIsNotFoundAndWaiterHasNoRoom()
    {
        //Arrange
        _roomRepositoryMock.ExistNameAsync(Arg.Is<string>(s => s == _command.Name)).Returns(false);
        _waiterRepositoryMock.SearchByIdAsync(Arg.Is<int>(id => id == _command.WaiterId)).Returns(new Waiter());

        //Act
        var result = await _handler.Handle(_command,default);

        //Assert
        result.IsSuccess.Should().BeTrue();
        result.Errors.Should().BeNull();
    }



}
