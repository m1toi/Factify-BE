using SocialMediaApp.BusinessLogic.Mapping;
using SocialMediaApp.DataAccess.Dtos.UserDto;
using SocialMediaApp.DataAccess.Repositories.UserRepository;

namespace SocialMediaApp.BusinessLogic.Services.UserService
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public void Register(UserRequestDto userDto)
        {
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(userDto.Password);
            userDto.Password = passwordHash;
            var user = userDto.ToUser();
            _userRepository.Register(user);
        }
        public List<UserResponseDto> GetAll()
        {
            var users = _userRepository.GetAll();
            var userResponseDtos = users.ToListUserResponseDto();
            return userResponseDtos;
        }

        public UserResponseDto GetById(int id)
        {
            var user = _userRepository.Get(id);
            var userResponseDto = user.ToUserResponseDto();
            return userResponseDto;
        }
        public void Update(int id, UserRequestDto updatedUserDto)
        {
            var updatedUser = updatedUserDto.ToUser();
            _userRepository.Update(id, updatedUser);
        }

        public void Delete(int id)
        {
            _userRepository.Delete(id);
        }
    }
}
