using SocialMediaApp.DataAccess.Dtos.CategoryDto;
using SocialMediaApp.DataAccess.Entity;

namespace SocialMediaApp.BusinessLogic.Mapping
{
	public static class CategoryMappingExtensions
	{
		public static CategoryResponseDto ToCategoryResponseDto(this Category category)
		{
			var categoryResponseDto = new CategoryResponseDto
			{
				CategoryId = category.CategoryId,
				Name = category.Name
			};
			return categoryResponseDto;
		}
	
		public static Category ToCategory(this CategoryRequestDto categoryRequestDto)
		{
			var category = new Category
			{
				Name = categoryRequestDto.Name
			};
			return category;
		}

		public static List<CategoryResponseDto> ToListCategoryResponseDto(this List<Category> categories)
		{
			var categoryResponseDtos = new List<CategoryResponseDto>();
			foreach (var category in categories)
			{
				categoryResponseDtos.Add(category.ToCategoryResponseDto());
			}
			return categoryResponseDtos;
		}
	}
}
