using Blogy.Business.Mappings;
using Blogy.Business.Services.BlogServices;
using Blogy.Business.Services.CategoryServices;
using Blogy.Business.Services.CommentServices;
using Blogy.Business.Validators.CategoryValidators;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogy.Business.Extensions
{
    public static class ServiceRegistrations
    {
        public static void AddServicesExt(this IServiceCollection service)
        {
            service.AddAutoMapper(typeof(CategoryMappings).Assembly);

            service.AddFluentValidationAutoValidation()
                .AddFluentValidationClientsideAdapters()
                .AddValidatorsFromAssembly(typeof(CreateCategoryValidator).Assembly);



            service.AddScoped<ICategoryService, CategoryService>();

            service.AddScoped<IBlogService, BlogService>();
            service.AddScoped<ICommentService, CommentService>();
        }
    }
}
