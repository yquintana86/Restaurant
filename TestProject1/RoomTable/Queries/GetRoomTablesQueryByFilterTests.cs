using Application.Abstractions.Repositories;
using Application.RoomTable.Queries.GetTablesbyFilter;
using FluentAssertions;
using NSubstitute;
using SharedLib.Models.Common;

namespace Application.UnitTests.RoomTable.Queries;

public class GetRoomTablesQueryByFilterTests
{

    private readonly GetRoomTablesByFilterQuery _filterQuery = new();
    private readonly IRoomTableRepository _roomTableRepositoryMock;
    private readonly GetRoomTablesByFilterQueryHandler _handler;

    public GetRoomTablesQueryByFilterTests()
    {
        _roomTableRepositoryMock = Substitute.For<IRoomTableRepository>();
        _handler = new (_roomTableRepositoryMock, default);
    }

    [Fact]
    public async Task Handler_Should_ReturnSuccess_WhenFilterCriteriaHasNoResult()
    {
        //Arrange
        _roomTableRepositoryMock.SearchByFilterAsync(_filterQuery).Returns(new PagedResult<Domain.Entities.RoomTable>() { Results = new List<Domain.Entities.RoomTable>()});

        //Act
        var result = await _handler.Handle(_filterQuery, default);

        //Assert
        result.Error.Should().BeNull();
        result.Results.Should().BeEmpty();
    }

}
