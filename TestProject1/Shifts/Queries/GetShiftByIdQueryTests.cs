using Application.Abstractions.Repositories;
using Application.Shifts.Queries.GetShiftById;
using Azure.Core;
using Domain.Entities;
using Domain.Exceptions;
using FluentAssertions;
using NSubstitute;
using SharedLib.Models.Common;
using System.Threading;

namespace Application.UnitTests.Shifts.Queries;

public class GetWaiterByIdQueryTests
{
    private readonly GetShiftByIdQuery _query = new (1);
    private readonly IShiftRepository _shiftRepositoryMock;
    private readonly GetShiftByIdQueryHandler _handler;

    public GetWaiterByIdQueryTests()
    {
       _shiftRepositoryMock = Substitute.For<IShiftRepository>();
       _handler = new (_shiftRepositoryMock,default);
    }

    [Fact]
    public async Task Handle_Should_ReturnError_WhenShiftNotFoundbyId()
    {
        //Arrange
        _shiftRepositoryMock.SearchByIdAsync(Arg.Is<int>(id => id == _query.ShiftId)).Returns((Shift?)null);

        //Act
        var result = await _handler.Handle(_query,default);

        //Assert
        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorType == ApiErrorType.NotFound);
    }

    [Fact]
    public async Task Handle_Should_ReturnSuccess_WhenShiftExistById()
    {
        //Arrange
        _shiftRepositoryMock.SearchByIdAsync(Arg.Is<int>(id => id == _query.ShiftId)).Returns(new Shift("", DateTime.Now, DateTime.Now.AddHours(2)));

        //Act
        var result = await _handler.Handle(_query, default);

        //Assert
        result.IsSuccess.Should().BeTrue();
        result.Errors.Should().BeNull();
    }


}
