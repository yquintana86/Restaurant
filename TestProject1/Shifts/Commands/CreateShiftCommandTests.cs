using Application.Abstractions.Repositories;
using Application.Shifts.Commands.CreateShift;
using Application.Shifts.Queries;
using Domain.Entities;
using Domain.Exceptions;
using FluentAssertions;
using NSubstitute;

namespace Application.UnitTests.Shifts.Commands;

public class CreateShiftCommandTests
{
    private static readonly CreateShiftCommand Command = new ("Firt", DateTime.Now, DateTime.Now.AddHours(2));

    private readonly CreateShiftCommandHandler _handler;
    private readonly IShiftRepository _shiftRepositoryMock;

    public CreateShiftCommandTests()
    {
        _shiftRepositoryMock = Substitute.For<IShiftRepository>();
        _handler = new (_shiftRepositoryMock, default!);
    }

    [Fact]
    public async Task Handle_Should_ReturnError_WhenCommandIsNull()
    {
        //Arrange

        CreateShiftCommand invalidCommand = null;

        //Act
        var result = await _handler.Handle(invalidCommand, default);


        //Assert
        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorType == SharedLib.Models.Common.ApiErrorType.Failure);
    }

    [Fact]
    public async Task Handle_Should_ReturnError_WhenShiftNotFoundByName()
    {
        //Arrange

        _shiftRepositoryMock.SearchByNameAsync(Arg.Is<string>(s => s == Command.Name)).Returns(Task.FromResult<Shift?>(new Shift("yo", DateTime.Now, DateTime.Now.AddHours(2))));

        //Act

        var result = await _handler.Handle(Command, default);

        //Assert

        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorType == SharedLib.Models.Common.ApiErrorType.Conflict);
    }

    [Fact]
    public async Task Handle_Should_ReturnSuccess_WhenShiftNameIsUnique()
    {
        //Arrange

        _shiftRepositoryMock.SearchByNameAsync(Command.Name).Returns((Shift?)null);

        //Act

        var result = await _handler.Handle(Command, default);

        //Assert

        result.IsSuccess.Should().BeTrue();
        result.Errors.Should().BeNull();
    }
}
