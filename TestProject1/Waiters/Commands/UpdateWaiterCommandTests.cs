using Application.Abstractions.Repositories;
using Application.Waiters.Commands.CreateWaiter;
using Application.Waiters.Commands.UpdateWaiter;
using Azure.Core;
using Domain.Entities;
using Domain.Exceptions;
using FluentAssertions;
using NSubstitute;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using System.Threading;

namespace Application.UnitTests.Waiters.Commands;

public class UpdateWaiterCommandTests
{
    private readonly UpdateWaiterCommand _command = new (1,"Yoel", "Quintana", 70M, 1);
    private readonly IWaiterRepository _waiterRepositoryMock;
    private readonly IShiftRepository _shiftRepositoryMock;
    private readonly UpdateWaiterCommandHandler _handler;

    public UpdateWaiterCommandTests()
    {
        _waiterRepositoryMock = Substitute.For<IWaiterRepository>();
        _shiftRepositoryMock = Substitute.For<IShiftRepository>();
        _handler = new (_waiterRepositoryMock, _shiftRepositoryMock, default);
    }

    [Fact]
    public async Task Handle_Should_ReturnError_WhenRelatedShiftIdNotFound()
    {
        //Arrange
        _shiftRepositoryMock.SearchByIdAsync(Arg.Is<int>(_command.Id)).Returns<Shift?>((Shift?)null);

        //Act
        var result = await _handler.Handle(_command, default);

        //Assert
        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorType == SharedLib.Models.Common.ApiErrorType.NotFound);
    }

    [Fact]
    public async Task Handle_Should_ReturnError_WhenWaiterNotFoundById()
    {
        //Arrange
        _shiftRepositoryMock.SearchByIdAsync(Arg.Is<int>(_command.Id)).Returns(new Shift());
        _waiterRepositoryMock.SearchByIdAsync(Arg.Is<int>(id => id == _command.Id))
                                                        .Returns((Waiter?)null);
        //Act
        var result = await _handler.Handle(_command, default);

        //Assert

        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorType == SharedLib.Models.Common.ApiErrorType.NotFound);
    }

    [Fact]
    public async Task Handle_Should_ReturnSuccess_WhenShiftIsFoundById()
    {
        //Arrange
        _shiftRepositoryMock.SearchByIdAsync(Arg.Is<int>(_command.Id)).Returns(new Shift());
        _waiterRepositoryMock.SearchByIdAsync(Arg.Is<int>(id => id == _command.Id)).Returns(new Waiter());

        //Act
        var result = await _handler.Handle(_command, default);

        //Assert
        result.IsSuccess.Should().BeTrue();
        result.Errors.Should().BeNull();
    }


}
