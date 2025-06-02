using SocialMediaApp.DataAccess.Dtos.PostDto;
using SocialMediaApp.DataAccess.Entity.Enums;
using System;

namespace SocialMediaApp.DataAccess.Dtos.ReportDto
{
	public class ReportResponseDto
	{
		public int ReportId { get; set; }
		public int PostId { get; set; }

		public PostResponseDto Post { get; set; }

		public int ReporterUserId { get; set; }
		public string ReporterUsername { get; set; }

		public ReportReason Reason { get; set; }
		public ReportStatus Status { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime? SolvedAt { get; set; }

		public int? AdminUserId { get; set; }
		public string AdminUsername { get; set; }
	}
}
