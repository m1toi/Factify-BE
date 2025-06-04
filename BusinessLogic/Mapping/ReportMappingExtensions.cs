using SocialMediaApp.DataAccess.Dtos.PostDto;
using SocialMediaApp.DataAccess.Dtos.ReportDto;
using SocialMediaApp.DataAccess.Entity;
using SocialMediaApp.DataAccess.Entity.Enums;
using System;

namespace SocialMediaApp.BusinessLogic.Mapping
{
	public static class ReportMappingExtensions
	{
		public static Report ToReport(this ReportRequestDto dto, int reporterUserId)
		{
			return new Report
			{
				PostId = dto.PostId,
				ReporterUserId = reporterUserId,
				Reason = dto.Reason,
				Status = ReportStatus.Pending,
				CreatedAt = DateTime.UtcNow
			};
		}

		public static ReportResponseDto ToReportResponseDto(this Report report)
		{

			return new ReportResponseDto
			{
				ReportId = report.ReportId,
				PostId = report.PostId,

				Post = new PostResponseDto
				{
					PostId = report.Post.PostId,
					Question = report.Post.Question,
					Answer = report.Post.Answer,
					CreatedAt = report.Post.CreatedAt,
					UserName = report.Post.User.Username,
					CategoryName = report.Post.Category.Name,
					UserId = report.Post.UserId,
					LikesCount = report.Post.Interactions?.Count(i => i.Liked) ?? 0,
					SharesCount = report.Post.Interactions?.Count(i => i.Shared) ?? 0
				},

				ReporterUserId = report.ReporterUserId,
				ReporterUsername = report.ReporterUser.Username,
				ReporterProfilePicture = report.ReporterUser?.ProfilePicture,

				Reason = report.Reason,
				Status = report.Status,
				CreatedAt = report.CreatedAt,
				SolvedAt = report.SolvedAt,

				AdminUserId = report.AdminUserId,
				AdminUsername = report.AdminUser?.Username ?? string.Empty
			};
		}

		public static List<ReportResponseDto> ToReportResponseDtoList(this List<Report> reports)
		{
			return reports.Select(r => r.ToReportResponseDto()).ToList();
		}
	}
}
