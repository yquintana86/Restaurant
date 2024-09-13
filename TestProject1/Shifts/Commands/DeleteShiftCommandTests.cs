using Application.Abstractions.Repositories;
using Application.Shifts.Commands.DeleteShift;
using Domain.Entities;
using Domain.Exceptions;
using FluentAssertions;
using NSubstitute;
using SharedLib.Models.Common;

namespace Application.UnitTests.Shifts.Commands;

public class DeleteShiftCommandTests
{
    private readonly DeleteShiftCommand _command = new (1);
    private readonly DeleteShiftCommandHandler _handler;
    private readonly IShiftRepository _shiftRepositoryMock;

    public DeleteShiftCommandTests()
    {
        _shiftRepositoryMock = Substitute.For<IShiftRepository>();
        _handler = new (_shiftRepositoryMock, default);
    }

    [Fact]
    public async Task Handler_Should_ReturnError_WhenShiftNotFoundById()
    {
        //Arrange
        _shiftRepositoryMock.SearchByIdAsync(Arg.Is<int>(id => id == _command.Id)).Returns((Shift?)null);

        //Act
        var result = await _handler.Handle(_command, default);

        //Assert
        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorType == ApiErrorType.NotFound);
    }

    [Fact]
    public async Task Handler_Should_ReturnError_WhenShiftHasWaiters()
    {
        //Arrange
        _shiftRepositoryMock.SearchByIdAsync(Arg.Is<int>(id => id == _command.Id)).Returns(new Shift { Waiters = new List<Waiter>() { new Waiter() }});

        //Act
        var result = await _handler.Handle(_command, default);

        //Assert
        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorType == ApiErrorType.Validation);
    }

    [Fact]
    public async Task Handle_Should_ReturnSuccess_WhenShiftIdExist()
    {
        //Arrange
        _shiftRepositoryMock.SearchByIdAsync(Arg.Is<int>(id => id == _command.Id)).Returns(new Shift(_command.Id, "", DateTime.Now, DateTime.Now));

        //Act
        var result = await _handler.Handle(_command, default);

        //Assert
        result.IsSuccess.Should().BeTrue();
        result.Errors.Should().BeNull();
    }


}
