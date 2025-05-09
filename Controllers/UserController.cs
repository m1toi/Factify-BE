using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using SocialMediaApp.BusinessLogic.Services.UserService;
using SocialMediaApp.DataAccess.Dtos.LoginDto;
using SocialMediaApp.DataAccess.Dtos.UserDto;
using SocialMediaApp.DataAccess.Entity;

namespace SocialMediaApp.Controllers
{
    [Authorize(Roles = "User")]
	[Route("/api/Users")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [AllowAnonymous]
		[HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public IActionResult Register([FromBody] UserRequestDto userDto)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _userService.Register(userDto);
            return Ok();
        }

		[AllowAnonymous]
		[HttpPost("login")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public ActionResult<string> Login([FromBody] LoginRequestDto loginDto)
		{
			if(!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
            string token = _userService.Login(loginDto);
            return Ok(token);
		}

		[HttpGet]
		//[Authorize(Roles = "User")]
		[ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public ActionResult<List<UserResponseDto>> GetAll()
        {
            return Ok(_userService.GetAll());
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<UserResponseDto> GetById([FromRoute] int id)
        {
            return Ok(_userService.GetById(id));
        }

		[HttpGet("current")]
		[Authorize(Roles = "User")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public ActionResult<UserResponseDto> GetCurrent()
		{
			var idClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			if (idClaim == null || !int.TryParse(idClaim, out var userId))
				return Unauthorized();

			var dto = _userService.GetById(userId);
			return Ok(dto);
		}


		[HttpPut("{id}")]        
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		public IActionResult Update([FromRoute] int id, [FromBody] UserRequestDto updatedUserDto)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _userService.Update(id, updatedUserDto);
            return NoContent();
        }

		[Authorize]                   
		[HttpPut("profile")]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		public IActionResult UpdateProfile([FromBody] UpdateProfileDto dto)
		{
			var idClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			if (!int.TryParse(idClaim, out var userId))
				return Unauthorized();

			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			try
			{
				_userService.UpdateProfile(userId, dto);
				return NoContent();
			}
			catch (Exception ex)
			{
				// you could inspect ex.Message or better yet throw a custom DuplicateUsernameException
				if (ex.Message.Contains("already taken"))
					return Conflict(new { error = ex.Message });
				throw;
			}
		}

		[HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		public IActionResult Delete([FromRoute] int id)
        {
            _userService.Delete(id);
            return NoContent();
        }
    }
}
