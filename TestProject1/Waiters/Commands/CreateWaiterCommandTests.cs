using Application.Abstractions.Repositories;
using Application.Waiters.Commands.CreateWaiter;
using Domain.Entities;
using FluentAssertions;
using NSubstitute;
using SharedLib.Models.Common;

namespace Application.UnitTests.Waiters.Commands;

public class CreateWaiterCommandTests
{
    private readonly CreateWaiterCommand _command = new ("Yoel", "Quintana", 70M, 1);
    private readonly IWaiterRepository _waiterRepositoryMock;
    private readonly IShiftRepository _shiftRepository;
    private readonly CreateWaiterCommandHandler _handler;

    public CreateWaiterCommandTests()
    {
        _waiterRepositoryMock = Substitute.For<IWaiterRepository>();
        _shiftRepository = Substitute.For<IShiftRepository>();
        _handler = new (_waiterRepositoryMock, default, _shiftRepository);
    }   

    [Fact]
    public async Task Handle_Should_ReturnError_WhenCommandShiftNotFound()
    {
        _shiftRepository.SearchByIdAsync(Arg.Is<int>(sh => sh == _command.ShiftId)).Returns((Shift?)null);

        var result = await _handler.Handle(_command, default);

        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorType == ApiErrorType.NotFound);
    }


    [Fact]
    public async Task Handle_Should_ReturnSucces_WhenWaiterShiftFound()
    {
        //Arrange
        _shiftRepository.SearchByIdAsync(Arg.Is<int>(sh => sh == _command.ShiftId)).Returns(new Shift());

        //Act
        var result = await _handler.Handle(_command, default);

        //Assert
        result.IsSuccess.Should().BeTrue();
        result.Errors.Should().BeNull();
    }


}
