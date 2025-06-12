using System.ComponentModel.DataAnnotations;

namespace SocialMediaApp.DataAccess.Dtos.PasswordDto
{
	public class ForgotPasswordDto
	{
		[Required, EmailAddress]
		public string Email { get; set; } = null!;
	}
}
