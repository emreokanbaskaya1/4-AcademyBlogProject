using Blogy.Business.DTOs.CategoryDtos;
using Blogy.Entity.Entities;

namespace Blogy.Business.Services.CategoryServices
{
    public interface ICategoryService
    {
        Task<List<ResultCategoryDto>> GetAllAsync();
        Task<UpdateCategoryDto> GetByIdAsync(int id);
        Task CreateAsync(CreateCategoryDto categoryDto);
        Task UpdateAsync(UpdateCategoryDto updateDto);
        Task DeleteAsync(int id);
    }
}
