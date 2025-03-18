using System.ComponentModel.DataAnnotations;

namespace SocialMediaApp.DataAccess.Dtos.LoginDto
{
	public class LoginRequestDto
	{
		public string Email { get; set; }
		public string Password { get; set; }

		public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
		{
			if (string.IsNullOrEmpty(Email))
			{
				yield return new ValidationResult("Email is required", new[] { nameof(Email) });
			}
			if (string.IsNullOrEmpty(Password))
			{
				yield return new ValidationResult("Password is required", new[] { nameof(Password) });
			}
		}
	}
}
