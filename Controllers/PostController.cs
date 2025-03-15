using Microsoft.AspNetCore.Mvc;
using SocialMediaApp.BusinessLogic.Services.PostService;
using SocialMediaApp.DataAccess.Dtos.PostDto;

namespace SocialMediaApp.Controllers
{
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
			_postService.Create(postDto);
			return Ok();
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
			_postService.Update(id, updatedPostDto);
			return NoContent();
		}

		[HttpDelete("{id}")]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status409Conflict)]
		public IActionResult Delete([FromRoute] int id)
		{
			_postService.Delete(id);
			return NoContent();
		}
	}
}
