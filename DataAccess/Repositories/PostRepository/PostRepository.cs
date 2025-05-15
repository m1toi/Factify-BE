using Microsoft.EntityFrameworkCore;
using SocialMediaApp.DataAccess.DataContext;
using SocialMediaApp.DataAccess.Entity;

namespace SocialMediaApp.DataAccess.Repositories.PostRepository
{
	public class PostRepository : BaseRepository, IPostRepository
	{
		public PostRepository(AppDbContext context) : base(context)
		{
		}
		public List<Post> GetAll()
		{
			return _context.Posts
						.Include(p => p.User)
						.Include(p => p.Category)  
						.ToList();
		}
		public Post Get(int id)
		{
			var post = _context.Posts
				.Include(p => p.User)
				.Include(p => p.Category) 
				.FirstOrDefault(p => p.PostId == id);

			if (post == null)
			{
				throw new Exception("Post not found");
			}

			return post;
		}

		public List<Post> GetByUser(int userId, int page, int pageSize)
		{
			return _context.Posts
				.Where(p => p.UserId == userId)
				.OrderByDescending(p => p.CreatedAt)
				.Skip((page - 1) * pageSize)
				.Take(pageSize)                
				.Include(p => p.User)
				.Include(p => p.Category)
				.ToList();
		}


		public Post Create(Post post)
		{
			if (!_context.Users.Any(u => u.UserId == post.UserId))
			{
				throw new ArgumentException("Invalid User ID.");
			}
			if (!_context.Categories.Any(c => c.CategoryId == post.CategoryId))
			{
				throw new ArgumentException("Invalid Category ID.");
			}
			if (_context.Posts.Any(p => p.Question == post.Question))
			{
				throw new Exception($"Post with question {post.Question} already exists");
			}

			_context.Posts.Add(post);
			_context.SaveChanges(); 

			return _context.Posts
				   .Include(p => p.User)
				   .Include(p => p.Category) 
				   .Single(p => p.PostId == post.PostId);
		}
		public Post Update(int id, Post post)
		{
			var postToUpdate = _context.Posts.Find(id);
			if (postToUpdate == null)
			{
				throw new Exception("Post not found");
			}

			if (!_context.Users.Any(u => u.UserId == post.UserId))
			{
				throw new ArgumentException("Invalid User ID.");
			}
			if (!_context.Categories.Any(c => c.CategoryId == post.CategoryId))
			{
				throw new ArgumentException("Invalid Category ID.");
			}
			if (_context.Posts.Any(p => p.Question == post.Question && p.PostId != id))
			{
				throw new Exception($"Post with question {post.Question} already exists");
			}

			postToUpdate.Question = post.Question;
			postToUpdate.Answer = post.Answer;
			postToUpdate.CategoryId = post.CategoryId;

			_context.SaveChanges();

			return _context.Posts
				.Include(p => p.User)
				.Include(p => p.Category) 
				.Single(p => p.PostId == id); 
		}
		public void Delete(int id)
		{
			var postToDelete = _context.Posts.Find(id);
			if(postToDelete == null)
			{
				throw new Exception($"Post with ID {id} not found");
			}
			_context.Posts.Remove(postToDelete);
			SaveChanges(); 
		}

		public List<Post> GetPostsByCategoriesExcludingSeen(List<int> categoryIds, List<int>seenPostIds, int totalPosts = 20)
		{
			return  _context.Posts
					.Where(p => categoryIds.Contains(p.CategoryId) && !seenPostIds.Contains(p.PostId))
					.Include(p => p.User)
					.Include(p => p.Category)
					.OrderByDescending(p => p.CreatedAt)
					.Take(totalPosts)
					.ToList();
		}
	}
}
