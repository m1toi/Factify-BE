using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialMediaApp.BusinessLogic.Services.ReportService;
using SocialMediaApp.DataAccess.Dtos.ReportDto;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace SocialMediaApp.Controllers
{
	[Authorize]
	[Route("api/Reports")]
	public class ReportsController : ControllerBase
	{
		private readonly IReportService _reportService;

		public ReportsController(IReportService reportService)
		{
			_reportService = reportService;
		}

		// POST api/reports
		[HttpPost]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		public IActionResult SubmitReport([FromBody] ReportRequestDto dto)
		{
			try
			{
				// ReporterUserId îl luăm din token
				var reporterUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
				_reportService.SubmitReport(reporterUserId, dto);
				return Ok(new { message = "Report submitted successfully." });
			}
			catch (Exception ex)
			{
				return BadRequest(new { error = ex.Message });
			}
		}

		[Authorize(Roles = "Admin")]
		[HttpGet("pending")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		[ProducesResponseType(StatusCodes.Status403Forbidden)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public ActionResult<List<ReportResponseDto>> GetPendingReports()
		{
			var reports = _reportService.GetPendingReports();
			return Ok(reports);
		}

		[Authorize(Roles = "Admin")]
		[HttpGet("{id}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		[ProducesResponseType(StatusCodes.Status403Forbidden)]
		public ActionResult<ReportResponseDto> GetReport(int id)
		{
			try
			{
				var report = _reportService.GetReportById(id);
				return Ok(report);
			}
			catch (Exception ex)
			{
				return NotFound(new { error = ex.Message });
			}
		}

		// PATCH api/reports/{id}/solve
		[Authorize(Roles = "Admin")]
		[HttpPatch("{id}/solve")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		[ProducesResponseType(StatusCodes.Status403Forbidden)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public IActionResult ResolveReport(int id, [FromQuery] bool deletePost = false)
		{
			try
			{
				var adminUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
				_reportService.ResolveReport(id, deletePost, adminUserId);
				return Ok(new { message = "Report resolved successfully." });
			}
			catch (Exception ex)
			{
				return BadRequest(new { error = ex.Message });
			}
		}
	}
}
