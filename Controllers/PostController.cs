using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialMediaApp.BusinessLogic.Services.PostService;
using SocialMediaApp.DataAccess.Dtos.PostDto;

namespace SocialMediaApp.Controllers
{
	[Authorize]
	[Route("api/Posts")]
	public class PostController : ControllerBase
	{
		private readonly IPostService _postService;

		public PostController(IPostService postService)
		{
			_postService = postService;
		}

		[HttpGet]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public ActionResult<List<PostResponseDto>> GetAll()
		{
			return Ok(_postService.GetAll());
		}

		[HttpGet("{id}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public ActionResult<PostResponseDto> GetById([FromRoute] int id)
		{
			return Ok(_postService.GetById(id));
		}

		[HttpPost]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status409Conflict)]
		public IActionResult Create([FromBody] PostRequestDto postDto)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			if (string.IsNullOrEmpty(userId))
			{
				return Unauthorized("Invalid user token.");
			}

			postDto.UserId = int.Parse(userId);

			PostResponseDto createdPost = _postService.Create(postDto);
			return CreatedAtAction(nameof(GetById), new { id = createdPost.PostId }, createdPost);
		}

		[HttpPut("{id}")]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status409Conflict)]
		public IActionResult Update([FromRoute] int id, [FromBody] PostRequestDto updatedPostDto)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			var existingPost = _postService.GetById(id);

			//if (existingPost == null)
			//{
			//	return NotFound("Post not found.");
			//}

			if (existingPost.UserId.ToString() != userId)
			{
				return Forbid("You are not authorized to update this post.");
			}
			PostResponseDto updatedPost = _postService.Update(id, updatedPostDto); 
			return Ok(updatedPost);
		}

		[HttpDelete("{id}")]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status409Conflict)]
		public IActionResult Delete([FromRoute] int id)
		{
			var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			var existingPost = _postService.GetById(id);

			//if (existingPost == null)
			//{
			//	return NotFound("Post not found.");
			//}

			// 🔒 Ensure only the post owner can delete it
			if (existingPost.UserId.ToString() != userId)
			{
				return Forbid("You are not authorized to delete this post.");
			}

			_postService.Delete(id);
			return NoContent();
		}
	}
}
