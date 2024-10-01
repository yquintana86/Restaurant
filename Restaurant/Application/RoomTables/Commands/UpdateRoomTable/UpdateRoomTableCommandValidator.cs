using FluentValidation;

namespace Application.RoomTable.Commands.UpdateTable;

public class UpdateRoomTableCommandValidator : AbstractValidator<UpdateRoomTableCommand>
{
    public UpdateRoomTableCommandValidator()
    {
        RuleFor(ut => ut.Id).GreaterThan(0); 
        RuleFor(ut => ut.TotalQty).GreaterThanOrEqualTo(2);
        RuleFor(ut => ut.RoomId).GreaterThan(0);
        RuleFor(ut => ut.WaiterId).Custom((id, ctx) =>
        {
            if (id is not null && id == 0)
                ctx.AddFailure(nameof(id), "Invalid WaiterId");
        });
    }
}

