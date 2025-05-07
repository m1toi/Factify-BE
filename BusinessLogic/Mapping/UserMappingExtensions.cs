using SocialMediaApp.DataAccess.Dtos.UserDto;
using SocialMediaApp.DataAccess.Entity;

namespace SocialMediaApp.BusinessLogic.Mapping
{
	public static class UserMappingExtensions
	{
		public static User ToUser(this UserRequestDto userRequestDto)
		{
			return new User
			{
				Username = userRequestDto.Name,
				Email = userRequestDto.Email,
				Password = userRequestDto.Password,
				ProfilePicture = "",
				RoleId = userRequestDto.RoleId
			};
		}

		public static UserResponseDto ToUserResponseDto(this User user)
		{
			return new UserResponseDto
			{
				Id = user.UserId,
				Name = user.Username,
				Email = user.Email,
				ProfilePicture = user.ProfilePicture,
				Role = user.Role.Name
			};
		}

		public static List<UserResponseDto> ToListUserResponseDto(this List<User> users)
		{
			return users.Select(user => user.ToUserResponseDto()).ToList();
		}
	}
}
