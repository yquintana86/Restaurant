using Application.Abstractions.Repositories;
using Application.Waiters.Commands.DeleteWaiter;
using Domain.Entities;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using SharedLib.Models.Common;

namespace Application.UnitTests.Waiters.Commands;

public class DeleteWaiterCommandTests
{
    private readonly DeleteWaiterCommand _command = new (1);
    private readonly IWaiterRepository _waiterRepositoryMock;
    private readonly DeleteWaiterCommandHandler _handler;

    public DeleteWaiterCommandTests()
    {
        _waiterRepositoryMock = Substitute.For<IWaiterRepository>();
        _handler = new (_waiterRepositoryMock, default);
    }

    [Fact]
    public async Task Handle_Should_ReturnError_WhenWaiterNotFoundById()
    {
        //Arrange
        _waiterRepositoryMock.SearchByIdAsync(Arg.Is<int>(id => id == _command.Id)).ReturnsNull();

        //Act
        var result = await _handler.Handle(_command, default);

        //Assert

        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorType == ApiErrorType.NotFound);
    }

    [Fact]
    public async Task Handle_Should_ReturnError_WhenWaiterHasRoomResponsibility()
    {
        //Arrange
        _waiterRepositoryMock.SearchByIdAsync(Arg.Is<int>(id => id == _command.Id))
                                                        .Returns(new Waiter() { Room = new Room() });
        //Act
        var result = await _handler.Handle(_command, default);

        //Assert

        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorType == ApiErrorType.Validation);

    }

    [Fact]
    public async Task Handle_Should_ReturnSuccess_WhenWaiterIsFoundById()
    {
        //Arrange
        _waiterRepositoryMock.SearchByIdAsync(Arg.Is<int>(id => id == _command.Id)).Returns(new Waiter());

        //Act
        var result = await _handler.Handle(_command, default);

        //Assert
        result.IsSuccess.Should().BeTrue();
        result.Errors.Should().BeNullOrEmpty();
    }


}
