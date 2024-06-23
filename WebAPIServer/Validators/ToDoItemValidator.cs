using FluentValidation;
using WebAPIServer.Models;

namespace WebAPIServer.Validators
{
    public class ToDoItemValidator : AbstractValidator<TODOItem>
    {
        public ToDoItemValidator()
        {
            RuleFor(x => x.PlannedDate).GreaterThanOrEqualTo(DateTime.Today);
        }
    }
}
