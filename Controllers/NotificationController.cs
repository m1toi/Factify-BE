using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialMediaApp.BusinessLogic.Services.NotificationService;
using SocialMediaApp.DataAccess.Entity;
using System.Security.Claims;

[Authorize]
[Route("api/notifications")]
public class NotificationController : ControllerBase
{
	private readonly INotificationService _notificationService;

	public NotificationController(INotificationService notificationService)
	{
		_notificationService = notificationService;
	}

	[HttpGet]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public IActionResult Get()
	{
		var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
		var notifications = _notificationService.GetUserNotifications(userId);
		return Ok(notifications);
	}

	[HttpPatch("{id}/read")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public IActionResult MarkAsRead(int id)
	{
		_notificationService.MarkNotificationAsRead(id);
		return NoContent();
	}
}
