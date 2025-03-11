using SocialMediaApp.DataAccess.Dtos.CategoryDto;

namespace SocialMediaApp.BusinessLogic.Services.CategoryService
{
    public interface ICategoryService
    {
        List<CategoryResponseDto> GetAll();
        CategoryResponseDto GetById(int id);
        void Create(CategoryRequestDto categoryDto);
        void Update(int id, CategoryRequestDto updatedCategoryDto);
        void Delete(int id);
    }
}   
