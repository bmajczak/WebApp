using System;
using WebApp.Models;

namespace WebApp.Services.Interfaces;

public interface IUserService
{
    Task<IEnumerable<User>> GetUsersAsync();
    Task<User> AddUserAsync(User user);
    Task<User> DeleteUserAsync(string userId);
    Task<User> UpdateUserAsync(User user);
    Task<User?> GetUserAsync(string id);
}
