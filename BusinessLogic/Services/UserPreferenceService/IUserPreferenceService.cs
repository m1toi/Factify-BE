using SocialMediaApp.DataAccess.Dtos.UserPreferenceDto;

namespace SocialMediaApp.BusinessLogic.Services.UserPreferenceService
{
	public interface IUserPreferenceService
	{
		void Create(UserPreferenceDto userPreferenceDto, int userId);

		bool HasPreference(int userId);
	}
}
