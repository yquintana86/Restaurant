using Application.Abstractions.Repositories;
using Application.Waiters.Commands.UpdateWaiter;
using Domain.Entities;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ReturnsExtensions;

namespace Application.UnitTests.Waiters.Commands;

public class UpdateWaiterCommandTests
{
    private readonly UpdateWaiterCommand _command = new (1,"Yoel", "Quintana",70M, DateTime.Now);
    private readonly IWaiterRepository _waiterRepositoryMock;
    private readonly UpdateWaiterCommandHandler _handler;

    public UpdateWaiterCommandTests()
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
        result.Errors.Should().Contain(e => e.ErrorType == SharedLib.Models.Common.ApiErrorType.NotFound);
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
