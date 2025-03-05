using SocialMediaApp.DataAccess.Dtos.RoleDto;
using SocialMediaApp.DataAccess.Entity;

namespace SocialMediaApp.BusinessLogic.Mapping
{
	public static class RoleMappingExtensions
	{
		public static RoleResponseDto ToRoleResponseDto(this Role role)
		{
			var roleResponseDto = new RoleResponseDto
			{
				RoleId = role.RoleId,
				Name = role.Name
			};
			return roleResponseDto;
		}	

		public static Role ToRole(this RoleRequestDto roleRequestDto)
		{
			var role = new Role
			{
				Name = roleRequestDto.Name
			};
			return role;
		}

		public static List<RoleResponseDto> ToListRoleResponseDto(this List<Role> roles)
		{
			var roleResponseDtos = new List<RoleResponseDto>();
			foreach (var role in roles)
			{
				roleResponseDtos.Add(role.ToRoleResponseDto());
			}
			return roleResponseDtos;
		}
	}
}
