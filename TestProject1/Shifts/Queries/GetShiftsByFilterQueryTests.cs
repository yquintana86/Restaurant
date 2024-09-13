using Application.Abstractions.Repositories;
using Application.Shifts.Queries.GetShiftsByFilter;
using Domain.Entities;
using FluentAssertions;
using NSubstitute;
using SharedLib.Models.Common;

namespace Application.UnitTests.Shifts.Queries;

public class GetWaitersByFilterQueryTests
{
    private readonly GetShiftsByFilterQuery getShiftsByFilterQuery = new ();
    private readonly IShiftRepository _shiftRepositoryMock;
    private readonly GetShiftsByFilterQueryHandler _handler;

    public GetWaitersByFilterQueryTests()
    {
       _shiftRepositoryMock = Substitute.For<IShiftRepository>();
        _handler = new (_shiftRepositoryMock, default);
    }

    [Fact]
    public async Task Handler_Should_ReturnNullResultField_WhenShiftNotFoundByFilter()
    {
        //Arrange
        _shiftRepositoryMock.SearchByFilterAsync(Arg.Is<GetShiftsByFilterQuery>(f => f.Name == getShiftsByFilterQuery.Name &&
                                                                                     f.Begin == getShiftsByFilterQuery.Begin &&
                                                                                     f.End == getShiftsByFilterQuery.End))
                                                                                .Returns(new PagedResult<Shift>());
        //Act
        var result = await _handler.Handle(getShiftsByFilterQuery, default);

        //Assert

        result.Error.Should().BeNull();
        result.Results.Should().BeNull();
    }
}
