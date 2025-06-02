using Microsoft.EntityFrameworkCore;
using SocialMediaApp.DataAccess.DataContext;
using SocialMediaApp.DataAccess.Entity;
using SocialMediaApp.DataAccess.Entity.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SocialMediaApp.DataAccess.Repositories.ReportRepository
{
	public class ReportRepository : IReportRepository
	{
		private readonly AppDbContext _context;

		public ReportRepository(AppDbContext context)
		{
			_context = context;
		}

		public Report CreateReport(Report report)
		{
			_context.Reports.Add(report);
			_context.SaveChanges();
			return report;
		}

		public Report GetPendingByPostId(int postId)
		{
			return _context.Reports
				.Include(r => r.ReporterUser)
				.Include(r => r.Post)
				.FirstOrDefault(r => r.PostId == postId && r.Status == DataAccess.Entity.Enums.ReportStatus.Pending);
		}

		public Report GetReportById(int reportId)
		{
			var report = _context.Reports
				.Include(r => r.ReporterUser)
				.Include(r => r.AdminUser)
				.Include(r => r.Post)
					.ThenInclude(p => p.User)
				.Include(r => r.Post)
					.ThenInclude(p => p.Category)
				.Include(r => r.Post)
					.ThenInclude(p => p.Interactions)
				.FirstOrDefault(r => r.ReportId == reportId);

			if (report == null)
				throw new Exception($"Report with ID {reportId} not found.");

			return report;
		}


		public List<Report> GetAllPending()
		{
			return _context.Reports
				.Include(r => r.ReporterUser)
				.Include(r => r.AdminUser)
				.Include(r => r.Post)
					.ThenInclude(p => p.User)
				.Include(r => r.Post)
					.ThenInclude(p => p.Category)
				.Include(r => r.Post)
					.ThenInclude(p => p.Interactions)
				.Where(r => r.Status == ReportStatus.Pending)
				.OrderByDescending(r => r.CreatedAt)
				.ToList();
		}


		public void UpdateReport(Report report)
		{
			_context.Reports.Update(report);
			_context.SaveChanges();
		}
	}
}
