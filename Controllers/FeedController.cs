using Microsoft.AspNetCore.Mvc;
using SocialMediaApp.BusinessLogic.Services.FeedService;

namespace SocialMediaApp.Controllers
{
	[Route("api/Feed")]
	public class FeedController : ControllerBase
	{
		private readonly IFeedService _feedService;

		public FeedController(IFeedService feedService)
		{
			_feedService = feedService;
		}

		[HttpGet("{userId}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public ActionResult GetPersonalizedFeed([FromRoute] int userId)
		{
			return Ok(_feedService.GetPersonalizedFeed(userId));
		}
	}
}
