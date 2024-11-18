using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebApp.Data;
using WebApp.Models;
using WebApp.Services.Services;
using WebApp.Services.Interfaces;
using Xunit;

namespace WebApp.Tests
{
    public class UserServiceTests : IDisposable
    {
        private readonly AppDbContext _context;
        private readonly IUserService _userService;

        public UserServiceTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase("TestDb") 
                .Options;

            _context = new AppDbContext(options); 
            _userService = new UserService(_context); 
        }

        [Fact]
        public async Task AddUserAsync_ShouldReturnUser_WhenUserIsAdded()
        {
            
            var user = new User
            {
                Id = "1",
                Name = "John",
                LastName = "Doe", 
                Email = "john@example.com"
            };

            var result = await _userService.AddUserAsync(user);


            Assert.NotNull(result); 
            Assert.Equal(user.Id, result.Id); 
            Assert.Equal(user.Name, result.Name); 
            Assert.Equal(user.LastName, result.LastName); 
            Assert.Equal(user.Email, result.Email); 

            var savedUser = await _context.Users.FindAsync(user.Id);
            Assert.NotNull(savedUser); 
            Assert.Equal(user.Name, savedUser.Name); 
            Assert.Equal(user.LastName, savedUser.LastName); 
            Assert.Equal(user.Email, savedUser.Email); 
        }
        public void Dispose()
        {
            _context.Database.EnsureDeleted(); 
            _context.Dispose();
        }
    }
}
