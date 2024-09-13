using Application.Abstractions.Repositories;
using Application.Shifts.Commands.UpdateShift;
using Domain.Entities;
using Domain.Exceptions;
using FluentAssertions;
using NSubstitute;
using SharedLib.Models.Common;

namespace Application.UnitTests.Shifts.Commands;

public class UpdateShiftCommandTests
{
    private readonly UpdateShiftCommand _command = new (1, "Second", DateTime.Now, DateTime.Now.AddHours(2));

    private readonly UpdateShiftCommandHandler _handler;

    private readonly IShiftRepository _shiftRepositoryMock;

    public UpdateShiftCommandTests()
    {
        _shiftRepositoryMock = Substitute.For<IShiftRepository>();
        _handler = new (_shiftRepositoryMock, default);
    }

    [Fact]
    public async Task Handle_Should_ReturnError_WhenCommandIsnull()
    {
        //Arrange
        UpdateShiftCommand invalidCommand = null;

        //Act

        var result = await _handler.Handle(invalidCommand, default);

        //Assert

        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorType == ApiErrorType.Failure);
    }

    [Fact]
    public async Task Handle_Should_ReturnError_WhenShiftNotFoundById()
    {
        //Arrange
        _shiftRepositoryMock.SearchByIdAsync(Arg.Is<int>(x => x == _command.Id)).Returns((Shift?)null);

        //Act

        var result = await _handler.Handle(_command, default);

        //Assert

        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorType == ApiErrorType.NotFound);
    }

    [Fact]
    public async Task Handle_Should_ReturnSuccess_WhenShiftExist()
    {
        //Arrange
        _shiftRepositoryMock.SearchByIdAsync(Arg.Is<int>(id => id == _command.Id)).Returns(new Shift(_command.Name, _command.Begin, _command.End));

        //Act
        var result = await _handler.Handle(_command, default);

        //Assert
        result.IsSuccess.Should().BeTrue();
        result.Errors.Should().BeNull();
    }
}
