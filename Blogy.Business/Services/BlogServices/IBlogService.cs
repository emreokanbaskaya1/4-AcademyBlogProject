﻿using Blogy.Business.DTOs.BlogDtos;
using Blogy.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogy.Business.Services.BlogServices
{
    public interface IBlogService: IGenericService<ResultBlogDto, UpdateBlogDto, CreateBlogDto>
    {
        Task<List<ResultBlogDto>> GetBlogWithCategoriesAsync();

        Task<List<ResultBlogDto>> GetBlogsByCategoryAsync(int categoryId);

        Task<List<ResultBlogDto>> GetLast3BlogsAsync();
    }
}
