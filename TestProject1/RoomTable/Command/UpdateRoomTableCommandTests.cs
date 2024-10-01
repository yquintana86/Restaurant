using Application.Abstractions.Repositories;
using Application.RoomTable.Commands.CreateTable;
using Application.RoomTable.Commands.UpdateTable;
using NSubstitute;
using SharedLib;
using Domain.Entities; 
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using System.Threading;
using Castle.DynamicProxy;
using NSubstitute.ReturnsExtensions;
using FluentAssertions;

namespace Application.UnitTests.RoomTable.Command;

public class UpdateRoomTableCommandTests
{
    private readonly UpdateRoomTableCommand _command = new ( 4, 1, RoomTableStatusType.Unreserved, default, 4);
    private readonly UpdateRoomTableCommandHandler _handler;
    private readonly IRoomTableRepository _roomTableRepositoryMock;

    public UpdateRoomTableCommandTests()
    {
        _roomTableRepositoryMock = Substitute.For<IRoomTableRepository>();
        _handler = new (_roomTableRepositoryMock,default);
    }

    [Fact]
    public async Task Handle_Should_ReturnError_WhenRoomTableNotFound()
    {
        //Arrange
         _roomTableRepositoryMock.SearchByIdAsync(_command.Id, _command.RoomId).Returns((Domain.Entities.RoomTable?)null);
        
        //Act
        var result = await _handler.Handle(_command,default);

        //Assert

        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorType == SharedLib.Models.Common.ApiErrorType.NotFound);
    }

    [Fact]
    public async Task Handle_Should_ReturnSuccess_WhenRoomTableIsFounded()
    {
        //Arrange
        _roomTableRepositoryMock.SearchByIdAsync(_command.Id, _command.RoomId).Returns(new Domain.Entities.RoomTable());

        //Act
        var result = await _handler.Handle(_command, default);

        //Assert
        result.IsSuccess.Should().BeTrue(); 
        result.Errors.Should().BeNull();

    }
}
