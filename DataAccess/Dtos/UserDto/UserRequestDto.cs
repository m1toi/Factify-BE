﻿namespace SocialMediaApp.DataAccess.Dtos.UserDto
{
	public class UserRequestDto
	{
		public string Name { get; set; }
		public string Email { get; set; }
		public string Password { get; set; }
		public int RoleId { get; set; }	
	}
}
