using SocialMediaApp.BusinessLogic.Mapping;
using SocialMediaApp.DataAccess.Dtos.ReportDto;
using SocialMediaApp.DataAccess.Entity;
using SocialMediaApp.DataAccess.Entity.Enums;
using SocialMediaApp.DataAccess.Repositories.PostRepository;
using SocialMediaApp.DataAccess.Repositories.ReportRepository;
using System;
using System.Collections.Generic;

namespace SocialMediaApp.BusinessLogic.Services.ReportService
{
	public class ReportService : IReportService
	{
		private readonly IReportRepository _reportRepository;
		private readonly IPostRepository _postRepository;

		public ReportService(IReportRepository reportRepo, IPostRepository postRepo)
		{
			_reportRepository = reportRepo;
			_postRepository = postRepo;
		}

		public void SubmitReport(int reporterUserId, ReportRequestDto dto)
		{
			var existing = _reportRepository.GetPendingByPostId(dto.PostId);
			if (existing != null)
			{
				throw new Exception("This post has already been reported by another user and will be reviewed shortly.");
			}

			var report = dto.ToReport(reporterUserId);

			_reportRepository.CreateReport(report);
		}

		public List<ReportResponseDto> GetPendingReports()
		{
			var pendingReports = _reportRepository.GetAllPending();
			return pendingReports.ToReportResponseDtoList();
		}

		public ReportResponseDto GetReportById(int reportId)
		{
			var report = _reportRepository.GetReportById(reportId);
			return report.ToReportResponseDto();
		}

		public void ResolveReport(int reportId, bool shouldDeletePost, int adminUserId)
		{
			var report = _reportRepository.GetReportById(reportId);
			if (report == null || report.Status == ReportStatus.Solved)
				throw new Exception("Report not found or already solved.");

			report.Status = ReportStatus.Solved;
			report.SolvedAt = DateTime.UtcNow;
			report.AdminUserId = adminUserId;

			_reportRepository.UpdateReport(report);

			if (shouldDeletePost)
			{
				_postRepository.Delete(report.PostId);
			}
		}
	}
}
