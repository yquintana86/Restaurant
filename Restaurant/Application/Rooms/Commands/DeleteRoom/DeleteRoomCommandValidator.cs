using FluentValidation;

namespace Application.Rooms.Commands.DeleteRoom;


public sealed class DeleteRoomCommandValidator : AbstractValidator<DeleteRoomCommand>
{
    public DeleteRoomCommandValidator()
    {
        RuleFor(c => c.id).GreaterThan(0);
    }
}

