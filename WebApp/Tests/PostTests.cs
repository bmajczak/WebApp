using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebApp.Data;
using WebApp.Models;
using Xunit;

namespace WebApp.Tests
{
    public class PostTests : IDisposable
    {
        private readonly AppDbContext _context;

        public PostTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase("TestDb")
                .Options;

            _context = new AppDbContext(options);
        }

        [Fact]
        public async Task AddPostAsync_ShouldReturnPost_WhenPostIsAdded()
        {
            var user = new User
            {
                Id = Guid.NewGuid().ToString(),
                Name = "John",
                LastName = "Doe",
                Email = "john@example.com"
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var post = new Post
            {
                Title = "Test Post",
                Content = "This is a test post.",
                UserId = user.Id
            };

            _context.Posts.Add(post);
            await _context.SaveChangesAsync();

            var savedPost = await _context.Posts.FindAsync(post.Id);
            Assert.NotNull(savedPost);
            Assert.Equal(post.Title, savedPost.Title);
            Assert.Equal(post.Content, savedPost.Content);
            Assert.Equal(post.UserId, savedPost.UserId);
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}
