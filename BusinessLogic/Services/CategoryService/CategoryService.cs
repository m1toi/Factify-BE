using SocialMediaApp.BusinessLogic.Mapping;
using SocialMediaApp.DataAccess.Dtos.CategoryDto;
using SocialMediaApp.DataAccess.Repositories.CategoryRepository;

namespace SocialMediaApp.BusinessLogic.Services.CategoryService
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public void Create(CategoryRequestDto categoryDto)
        {
            var category = categoryDto.ToCategory();
            _categoryRepository.Create(category);
        }

        public void Delete(int id)
        {
            _categoryRepository.Delete(id);
        }

        public List<CategoryResponseDto> GetAll()
        {
            var categories = _categoryRepository.GetAll();
            var categoryResponseDtos = categories.ToListCategoryResponseDto();
            return categoryResponseDtos;

        }

        public CategoryResponseDto GetById(int id)
        {
            var category = _categoryRepository.Get(id);
            var categoryResponseDto = category.ToCategoryResponseDto();
            return categoryResponseDto;
        }

        public void Update(int id, CategoryRequestDto updatedCategoryDto)
        {
            var category = updatedCategoryDto.ToCategory();
            category.CategoryId = id;
            _categoryRepository.Update(id, category);
        }
    }
}
