using Blogy.Business.DTOs.BlogDtos;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogy.Business.Validators.BlogValidators
{
    public class CreateBlogValidator: AbstractValidator<CreateBlogDto>
    {
        public CreateBlogValidator()
        {
            RuleFor(x=>x.Title).NotEmpty().WithMessage("Title can not be left blank.");
            RuleFor(x=>x.Description).NotEmpty().WithMessage("Description can not be left blank.");
            RuleFor(x=>x.CoverImage).NotEmpty().WithMessage("Cover Image can not be left blank.");
            RuleFor(x=>x.BlogImage1).NotEmpty().WithMessage("Blog Image 1 can not be left blank.");
            RuleFor(x=>x.BlogImage2).NotEmpty().WithMessage("Blog Image 2 can not be left blank.");
            RuleFor(x=>x.CategoryId).NotEmpty().WithMessage("Category can not be left blank.");
        }
    }
}
