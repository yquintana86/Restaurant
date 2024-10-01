using Application.Abstractions.Repositories;
using Application.Waiters.Queries.GetWaiterById;
using Domain.Entities;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using SharedLib.Models.Common;

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
        _waiterRepositoryMock.SearchByIdAsync(Arg.Is<int>(id => id == _query.Id)).ReturnsNull();

        //Act
        var result = await _handler.Handle(_query, default);

        //Assert
        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorType == ApiErrorType.NotFound);
    }

    [Fact]
    public async Task Handle_Should_ReturnSuccess_WhenWaiterExistById()
    {
        //Arrange
        _waiterRepositoryMock.SearchByIdAsync(Arg.Is<int>(id => id == _query.Id)).Returns(new Waiter());

        //Act
        var result = await _handler.Handle(_query, default);

        //Assert
        result.IsSuccess.Should().BeTrue();
        result.Errors.Should().BeNullOrEmpty();
    }


}
