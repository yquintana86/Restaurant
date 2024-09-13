using Application.Abstractions.Repositories;
using Application.Rooms.Commands.DeleteRoom;
using Azure.Core;
using Domain.Entities;
using Domain.Exceptions;
using FluentAssertions;
using NSubstitute;

namespace Application.UnitTests.Rooms.Commands;

public sealed class DeleteRoomCommandTests
{
    private readonly DeleteRoomCommand _command = new (1);
    private readonly DeleteRoomCommandHandler _handler;
    private readonly IRoomRepository _roomRepositoryMock;

    public DeleteRoomCommandTests()
    {
        _roomRepositoryMock = Substitute.For<IRoomRepository>();
        _handler = new (_roomRepositoryMock, default); 
    }

    [Fact]
    public async Task Handle_Should_ReturnError_WhenRoomNotFoundedById()
    {
        //Arrange
        _roomRepositoryMock.ExistIdAsync(Arg.Is<int>(s => s == _command.id)).Returns(false);

        //Act
        var result = await _handler.Handle(_command,default);

        //Assert
        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().Contain(e => e.Message == RoomError.InvalidId(_command.id).Message);
    }

    [Fact]
    public async Task Handle_Should_ReturnSuccess_WhenRoomFoundedById()
    {
        //Arrange
        _roomRepositoryMock.ExistIdAsync(Arg.Is<int>(s => s == _command.id)).Returns(true);

        //Act
        var result = await _handler.Handle(_command,default);

        //Assert

        result.IsSuccess.Should().BeTrue();
        result.Errors.Should().BeNull();
    }



}
