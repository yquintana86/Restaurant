using Application.Abstractions.Repositories;
using Application.Rooms.Commands.DeleteRoom;
using Domain.Entities;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using SharedLib.Models.Common;

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
        _roomRepositoryMock.SearchByIdAsync(_command.id, default).ReturnsNull();

        //Act
        var result = await _handler.Handle(_command,default);

        //Assert
        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorType == ApiErrorType.Validation);
    }

    [Fact]
    public async Task Handle_Should_ReturnError_WhenRoomStillHasTables()
    {
        //Arrange
        _roomRepositoryMock.SearchByIdAsync(_command.id, default)
            .Returns(new Room { Tables = new List<Domain.Entities.RoomTable>() 
                                                    { new Domain.Entities.RoomTable()} });

        //Act
        var result = await _handler.Handle(_command, default);

        //Assert
        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorType == ApiErrorType.Validation);
    }

    [Fact]
    public async Task Handle_Should_ReturnSuccess_WhenRoomFoundedById()
    {
        //Arrange
        _roomRepositoryMock.ExistIdAsync(_command.id).Returns(true);
        _roomRepositoryMock.SearchByIdAsync(_command.id, default).Returns(new Room());

        //Act
        var result = await _handler.Handle(_command,default);

        //Assert

        result.IsSuccess.Should().BeTrue();
        result.Errors.Should().BeNullOrEmpty();
    }



}
