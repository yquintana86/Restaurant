using FluentValidation;

namespace Application.Rooms.Commands.CreateRoom;

public sealed class CreateRoomCommandValidator : AbstractValidator<CreateRoomCommand>
{
    public CreateRoomCommandValidator()
    {
        RuleFor(r => r.Name).NotEmpty();
        RuleFor(r => r.WaiterId).GreaterThan(0);
    }
}