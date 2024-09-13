using FluentValidation;

namespace Application.Waiters.Queries.GetWaiterById;

public sealed class GetWaiterByIdQueryValidator : AbstractValidator<GetWaiterByIdQuery>
{
    public GetWaiterByIdQueryValidator()
    {
        RuleFor(w => w.Id).GreaterThan(0);
    }
}
