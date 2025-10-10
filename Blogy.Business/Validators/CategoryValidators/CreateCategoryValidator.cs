using Blogy.Business.DTOs.CategoryDtos;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogy.Business.Validators.CategoryValidators
{
    public class CreateCategoryValidator: AbstractValidator<CreateCategoryDto>
    {
        public CreateCategoryValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Category Name is required").MinimumLength(3).WithMessage("Category Name must be at least 3 characters.").MaximumLength(50).WithMessage("Category Name must be maximum 50 characters.");
        }
    }
}
