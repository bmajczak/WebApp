using System;
using WebApp.Models;

namespace WebApp.Services.Interfaces;

public interface IUserService
{
    public IEnumerable<User> GetUsers();
    public User AddUser(User user);
    public User DeleteUser(User user);
    public User UpdateUser(User user);
    public User GetUser(int id);
}
