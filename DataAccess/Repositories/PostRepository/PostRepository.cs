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
			return _context.Posts.ToList();
		}
		public Post Get(int id)
		{
			var post = _context.Posts.Find(id);
			if (post == null)
			{
				throw new Exception("Post not found");
			}
			return post;
		}
		public void Create(Post post)
		{
			if(_context.Posts.Any(p => p.Question == post.Question))
			{
				throw new Exception($"Post with question {post.Question} already exists");
			}
			_context.Posts.Add(post);
			SaveChanges();
		}
		public void Update(int id, Post post)
		{
			var postToUpdate = _context.Posts.Find(id);
			if (postToUpdate == null)
			{
				throw new Exception("Post not found");
			}
			var category = _context.Categories.Find(post.CategoryId);
			if (category == null)
			{
				throw new Exception("Category not found");
			}
			var user = _context.Users.Find(post.UserId);
			if (user == null)
			{
				throw new Exception("User not found");
			}
			if (_context.Posts.Any(p => p.Question == post.Question && p.PostId != id))
			{
				throw new Exception($"Post with question {post.Question} already exists");
			}
			postToUpdate.Question = post.Question;
			postToUpdate.Answer = post.Answer;
			postToUpdate.CategoryId = post.CategoryId;
			SaveChanges();
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
					.OrderByDescending(p => p.CreatedAt)
					.Take(totalPosts)
					.ToList();
		}
	}
}
