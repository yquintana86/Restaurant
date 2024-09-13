using Application.Abstractions.Repositories;
using Application.Shifts.Queries.GetShiftById;
using Application.Waiters.Queries.GetWaiterById;
using Azure.Core;
using Domain.Entities;
using Domain.Exceptions;
using FluentAssertions;
using NSubstitute;
using SharedLib.Models.Common;
using System.Threading;

namespace Application.UnitTests.Waiters.Queries;

public class GetWaiterByIdQueryTests
{
    private readonly GetWaiterByIdQuery _query = new (1);
    private readonly IWaiterRepository _waiterRepositoryMock;
    private readonly GetWaiterByIdQueryHandler _handler;

    public GetWaiterByIdQueryTests()
    {
        _waiterRepositoryMock = Substitute.For<IWaiterRepository>();
        _handler = new (_waiterRepositoryMock, default);
    }

    [Fact]
    public async Task Handle_Should_ReturnError_WhenWaiterIdRequestedLessOrEqualThanZero()
    {
        //Arrange

        //Act
        var result = await _handler.Handle(new GetWaiterByIdQuery(0), default);

        //Assert
        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorType == ApiErrorType.Validation);
    }

    [Fact]
    public async Task Handle_Should_ReturnError_WhenWaiterNotFoundbyId()
    {
        //Arrange
        _waiterRepositoryMock.SearchByIdAsync(Arg.Is<int>(id => id == _query.Id)).Returns((Waiter?)null);

        //Act
        var result = await _handler.Handle(_query, default);

        //Assert
        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorType == ApiErrorType.NotFound);
    }

    [Fact]
    public async Task Handle_Should_ReturnSuccess_WhenShiftExistById()
    {
        //Arrange
        _waiterRepositoryMock.SearchByIdAsync(Arg.Is<int>(id => id == _query.Id)).Returns(new Waiter());

        //Act
        var result = await _handler.Handle(_query, default);

        //Assert
        result.IsSuccess.Should().BeTrue();
        result.Errors.Should().BeNull();
    }


}
