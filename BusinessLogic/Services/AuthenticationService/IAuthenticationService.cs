using SocialMediaApp.DataAccess.Entity;

namespace SocialMediaApp.BusinessLogic.Services.AuthenticationService
{
	public interface IAuthenticationService
	{
		string GenerateToken(User user);
	}
}
