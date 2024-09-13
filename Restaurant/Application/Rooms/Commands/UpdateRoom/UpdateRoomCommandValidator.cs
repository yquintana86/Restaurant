using FluentValidation;

namespace Application.Rooms.Commands.UpdateRoom;

public sealed class UpdateRoomCommandValidator : AbstractValidator<UpdateRoomCommand>
{
    public UpdateRoomCommandValidator()
    {
        RuleFor(r => r.Id).GreaterThan(0);
        RuleFor(r => r.WaiterId).GreaterThan(0);
    }
}
