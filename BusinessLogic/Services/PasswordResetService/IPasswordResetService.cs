namespace SocialMediaApp.BusinessLogic.Services.PasswordResetService
{
	public interface IPasswordResetService
	{
		Task RequestPasswordResetAsync(string email);
		Task ResetPasswordAsync(string token, string newPassword);
	}

}
