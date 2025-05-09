using System.ComponentModel.DataAnnotations;

namespace SocialMediaApp.DataAccess.Dtos.UserDto
{
	public class UpdateProfileDto
	{
		[Required]
		[StringLength(30, MinimumLength = 2,
		ErrorMessage = "Username must be between 2 and 30 characters.")]
		[RegularExpression(@"^[A-Za-z0-9_.]+$",
		ErrorMessage = "Usernames can only contain letters, numbers, underscores and periods.")]
		public string Name { get; set; }
		public string ProfilePicture { get; set; }
	}
}