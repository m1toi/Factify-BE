using SocialMediaApp.BusinessLogic.Mapping;
using SocialMediaApp.DataAccess.Dtos.LoginDto;
using SocialMediaApp.DataAccess.Dtos.UserDto;
using SocialMediaApp.DataAccess.Repositories.UserRepository;

namespace SocialMediaApp.BusinessLogic.Services.UserService
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly AuthenticationService.IAuthenticationService _authenticationService;

		public UserService(IUserRepository userRepository, AuthenticationService.IAuthenticationService authenticationService)
        {
            _userRepository = userRepository;
            _authenticationService = authenticationService;
        }

        public void Register(UserRequestDto userDto)
        {
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(userDto.Password);
            userDto.Password = passwordHash;
            var user = userDto.ToUser();
            _userRepository.Register(user);
        }

        public string Login(LoginRequestDto loginDto)
        {
            var user = _userRepository.GetByEmail(loginDto.Email);
            if(!BCrypt.Net.BCrypt.Verify(loginDto.Password, user.Password))
			{
				throw new Exception("Invalid password");
			}
            string token = _authenticationService.GenerateToken(user);
            return token;
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
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(updatedUserDto.Password);
            updatedUserDto.Password = passwordHash;
            var updatedUser = updatedUserDto.ToUser();
            _userRepository.Update(id, updatedUser);
        }

        public void Delete(int id)
        {
            _userRepository.Delete(id);
        }
    }
}
