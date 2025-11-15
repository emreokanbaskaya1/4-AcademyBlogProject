using Blogy.Entity.Entities;
using FluentValidation;

namespace Blogy.Business.Validators
{
    public class CommentValidator : AbstractValidator<Comment>
    {
        public CommentValidator()
        {
            RuleFor(x => x.UserId).NotEmpty().WithMessage("Username can not be blank.");
            RuleFor(x => x.BlogId).NotEmpty().WithMessage("Blog can not be blank.");
            RuleFor(x => x.Content).NotEmpty().WithMessage("Comment can not be blank.")
                                   .MaximumLength(250).WithMessage("Comment can not exceed 250 characters.");
        }
    }
}
