﻿using Blogvio.WebApi.Data;
using Blogvio.WebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace Blogvio.WebApi.Repositories
{
	public class BlogRepository : IBlogRepository
	{
		private readonly AppDbContext _context;

		public BlogRepository(AppDbContext context)
		{
			_context = context;
		}

		public async Task CreateBlogAsync(Blog blog)
		{
			await _context.Blogs.AddAsync(blog);
		}

		public async Task DeleteBlogAsync(int id)
		{
			//Blog blog = new Blog() { Id = id };
			//_context.Blogs.Attach(blog);
			//_context.Blogs.Remove(blog);
			var blog = await GetBlogAsync(id);
			blog.IsDeleted = true;
			await Task.CompletedTask;
		}

		public async Task<Blog> GetBlogAsync(int id)
		{
			return await _context
				.Blogs
				.Where(b => b.Id == id &&
				!b.IsDeleted)
				.FirstOrDefaultAsync();
		}

		public async Task<IEnumerable<Blog>> GetBlogsAsync()
		{
			return await _context.Blogs.ToListAsync();
		}

		public async Task<bool> SaveChangesAsync()
		{
			return (await _context.SaveChangesAsync() >= 0);
		}

		public async Task UpdateBlogAsync(Blog blog)
		{
			_context.Entry(
				await _context.Blogs
					.FirstOrDefaultAsync(b => b.Id == blog.Id))
				.CurrentValues
				.SetValues(blog);
		}
	}
}