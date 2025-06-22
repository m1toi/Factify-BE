using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using SocialMediaApp.BusinessLogic.Services.PasswordResetService;
using SocialMediaApp.BusinessLogic.Services.PostService;
using SocialMediaApp.BusinessLogic.Services.UserService;
using SocialMediaApp.DataAccess.Dtos.LoginDto;
using SocialMediaApp.DataAccess.Dtos.PasswordDto;
using SocialMediaApp.DataAccess.Dtos.PostDto;
using SocialMediaApp.DataAccess.Dtos.UserDto;
using SocialMediaApp.DataAccess.Entity;
using SocialMediaApp.DataAccess.Repositories.UserRepository;

namespace SocialMediaApp.Controllers
{
    [Authorize]
	[Route("/api/Users")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
		private readonly IPostService _postService;
		private readonly IPasswordResetService _passwordResetService;
		public UserController(IUserService userService, IPostService postService, IPasswordResetService passwordResetService)
        {
            _userService = userService;
			_postService = postService;
			_passwordResetService = passwordResetService;
		}

        [AllowAnonymous]
		[HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public IActionResult Register([FromBody] UserRequestDto userDto)
        {
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			try
			{
				_userService.Register(userDto);
				return Ok();
			}
			catch (Exception ex)
			{
				if (ex.Message.Contains("already exists"))
				{
					// dacă e username, expui exact “username-ul există deja”
					if (ex.Message.Contains("username"))
						return Conflict(new { error = "Username already exists." });
					// dacă e email, nu divulgăm că email-ul există
					return Conflict(new { error = "Could not create account. Try a different email" });
				}
				return StatusCode(500, new { error = "Server error. Please try again later." });
			}
		}

		[AllowAnonymous]
		[HttpPost("login")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public ActionResult<string> Login([FromBody] LoginRequestDto loginDto)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			try
			{
				var token = _userService.Login(loginDto);
				return Ok(token);
			}
			catch (Exception ex)
			{
				// Poţi filtra după tipul excepţiei (ideea e să nu prindem System.Exception generic)
				if (ex.Message.Contains("not found") || ex.Message.Contains("Invalid password"))
					// expui doar un mesaj comun
					return BadRequest(new { error = "Invalid email or password" });
				// alt gen de eroare
				return StatusCode(500, new { error = "Server error. Please try again later." });
			}
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

		[HttpGet("{id}/posts")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public ActionResult<List<PostResponseDto>> GetPostsByUser(
			[FromRoute] int id,
			[FromQuery] int page = 1,
			[FromQuery] int pageSize = 10  
		)
		{
			// 1) Poți verifica existența user-ului și returna NotFound dacă lipsește.
			var posts = _postService.GetByUser(id, page, pageSize);
			return Ok(posts);
		}

		[HttpGet("search")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		public ActionResult<List<UserSearchResultDto>> SearchByUsername([FromQuery] string query)
		{
			var idClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			if (!int.TryParse(idClaim, out var currentUserId))
				return Unauthorized();

			var results = _userService.SearchByUsername(query, currentUserId);
			return Ok(results);
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

		// POST api/Users/forgot-password
		[AllowAnonymous]
		[HttpPost("forgot-password")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto dto)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			await _passwordResetService.RequestPasswordResetAsync(dto.Email);
			return Ok(new
			{
				message = "If that email exists, you will receive instructions to reset your password."
			});
		}

		// POST api/Users/reset-password
		[AllowAnonymous]
		[HttpPost("reset-password")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto dto)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			try
			{
				await _passwordResetService.ResetPasswordAsync(dto.Token, dto.NewPassword);
				return Ok(new { message = "Your password has been reset." });
			}
			catch (Exception ex)
			{
				// poți returna 400 cu un mesaj concret
				return BadRequest(new { error = ex.Message });
			}
		}

	}
}
