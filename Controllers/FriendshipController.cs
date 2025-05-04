using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialMediaApp.BusinessLogic.Services.FriendshipService;
using SocialMediaApp.DataAccess.Dtos.FriendshipDto;
using System.Security.Claims;

namespace SocialMediaApp.Controllers
{
	[Authorize]
	[Route("api/Friendships")]
	public class FriendshipController : ControllerBase
	{
		private readonly IFriendshipService _friendshipService;

		public FriendshipController(IFriendshipService friendshipService)
		{
			_friendshipService = friendshipService;
		}

		[HttpGet("user/{userId}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public ActionResult<List<FriendshipResponseDto>> GetUserFriendships([FromRoute] int userId)
		{
			var friendships = _friendshipService.GetUserFriendships(userId);
			return Ok(friendships);
		}

		[HttpGet("check/{friendId}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public ActionResult<bool> CheckFriendship([FromRoute] int friendId)
		{
			var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
			var areFriends = _friendshipService.AreUsersFriends(userId, friendId);
			return Ok(areFriends);
		}

		[HttpPost]
		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public IActionResult CreateFriendship([FromBody] FriendshipRequestDto friendshipDto)
		{
			var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

			if (friendshipDto.UserId != userId)
				return Forbid("You can only create friendships for yourself.");

			try
			{
				var createdFriendship = _friendshipService.CreateFriendship(friendshipDto);
				return CreatedAtAction(nameof(GetUserFriendships), new { userId = userId }, createdFriendship);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		[HttpPatch("{friendshipId}/accept")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status403Forbidden)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public IActionResult AcceptFriendRequest([FromRoute] int friendshipId)
		{
			var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

			var friendship = _friendshipService.GetFriendship(friendshipId);

			if (friendship == null)
				return NotFound("Friendship not found.");

			if (friendship.FriendId != userId)
				return Forbid("You are not authorized to accept this friend request.");

			if (friendship.IsConfirmed)
				return BadRequest("Friendship already confirmed.");

			_friendshipService.AcceptFriendRequest(friendshipId);

			return Ok("Friend request accepted.");
		}

		[HttpDelete("{friendshipId}")]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status403Forbidden)]
		public IActionResult DeleteFriendship([FromRoute] int friendshipId)
		{
			var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
			var friendship = _friendshipService.GetFriendship(friendshipId);

			if (friendship.UserId != userId && friendship.FriendId != userId)
				return Forbid("You are not authorized to delete this friendship.");

			_friendshipService.DeleteFriendship(friendshipId);
			return NoContent();
		}
	}
}
