using Application.Abstractions.Repositories;
using Application.RoomTable.Commands.DeleteTable;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using FluentAssertions;
using SharedLib;
using SharedLib.Models.Common;


namespace Application.UnitTests.RoomTable.Command;

public class DeleteRoomTableCommandTests
{
    private readonly DeleteRoomTableCommand _command = new (1,1);
    private readonly IRoomTableRepository _roomTableRepositoryMock;
    private readonly DeleteRoomTableCommandHandler _handler;

    public DeleteRoomTableCommandTests()
    {
        _roomTableRepositoryMock = Substitute.For<IRoomTableRepository>();
        _handler = new (_roomTableRepositoryMock, default);
    }

    [Fact]
    public async Task Handler_Should_ReturnError_WhenRoomCommandNotFound()
    {
        //Arrange
        _roomTableRepositoryMock.SearchByIdAsync(_command.Id, _command.RoomId, default).ReturnsNull();

        //Act
        var result = await _handler.Handle(_command, default);

        //Assert

        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorType == ApiErrorType.NotFound);

    }

    [Fact]
    public async Task Handle_Should_ReturnError_WhenTableHasActiveReservations()
    {

        //Arrange

        _roomTableRepositoryMock.SearchByIdAsync(_command.Id, _command.RoomId, default)
                                    .Returns(new Domain.Entities.RoomTable() {
                                                    Status = RoomTableStatusType.Reserved });


        //Act
        var result = await _handler.Handle(_command, default);

        //Assert
        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorType == ApiErrorType.Validation);
    }


    [Fact]
    public async Task Handler_Should_ReturnSuccess_WhenRoomCommandNotFound()
    {
        //Arrange
        _roomTableRepositoryMock.SearchByIdAsync(_command.Id, _command.RoomId, default).Returns(new Domain.Entities.RoomTable());

        //Act
        var result = await _handler.Handle(_command, default);

        //Assert

        result.IsSuccess.Should().BeTrue();
        result.Errors.Should().BeNullOrEmpty();
    }





}
