using SocialMediaApp.DataAccess.Dtos.LoginDto;
using SocialMediaApp.DataAccess.Dtos.UserDto;

namespace SocialMediaApp.BusinessLogic.Services.UserService
{
    public interface IUserService
    {
        void Register(UserRequestDto userDto);
		string Login(LoginRequestDto loginDto);
		List<UserResponseDto> GetAll();
        UserResponseDto GetById(int id);
        void Update(int id, UserRequestDto updatedUserDto);
        void Delete(int id);
    }
}
