using Application.Abstractions.Repositories;
using Application.Waiters.Queries.GetWaitersByFilter;
using Domain.Entities;
using FluentAssertions;
using NSubstitute;
using SharedLib.Models.Common;

namespace Application.UnitTests.Waiters.Queries;

public class GetWaitersByFilterQueryTests
{
    private readonly GetWaitersByFilterQuery getWaitersByFilterQuery = new ();
    private readonly IWaiterRepository _waiterRepositoryMock;
    private readonly GetWaitersByFilterQueryHandler _handler;

    public GetWaitersByFilterQueryTests()
    {
        _waiterRepositoryMock = Substitute.For<IWaiterRepository>();
        _handler = new (_waiterRepositoryMock, default);
    }

    [Fact]
    public async Task Handler_Should_ReturnSuccess_WhenFilterCriteriaGetsPagedResult()
    {
        //Arrange
        _waiterRepositoryMock.SearchByFilterAsync(Arg.Is<GetWaitersByFilterQuery>(f => f.FirstName == getWaitersByFilterQuery.FirstName &&
                                                                                     f.LastName == getWaitersByFilterQuery.LastName &&
                                                                                     f.SalaryTo == getWaitersByFilterQuery.SalaryTo &&
                                                                                     f.SalaryFrom == getWaitersByFilterQuery.SalaryFrom))
                                                                                .Returns(new PagedResult<Waiter>());
        //Act
        var result = await _handler.Handle(getWaitersByFilterQuery, default);

        //Assert
        result.Error.Should().BeNull();
        result.Results.Should().BeNullOrEmpty();
    }


}
