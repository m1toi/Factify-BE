using SocialMediaApp.DataAccess.Dtos.UserPreferenceDto;
using SocialMediaApp.DataAccess.Entity;
using SocialMediaApp.DataAccess.Repositories.UserCategoryRepository;

namespace SocialMediaApp.BusinessLogic.Services.UserPreferenceService
{
	public class UserPreferenceService : IUserPreferenceService
	{
		private readonly IUserCategoryRepository _userCategoryRepository;

		public UserPreferenceService(IUserCategoryRepository userCategoryRepository)
		{
			_userCategoryRepository = userCategoryRepository;
		}

		public void Create(UserPreferenceDto userPreferenceDto, int userId)
		{
			foreach (var categoryId in userPreferenceDto.CategoryIds)
			{
				var existing = _userCategoryRepository.Get(userId, categoryId);

				if (existing == null)
				{
					var preference = new UserCategoryPreference
					{
						UserId = userId,
						CategoryId = categoryId,
						Score = 0.5
					};
					_userCategoryRepository.Add(preference);
				}
			}

		}
	}
}
