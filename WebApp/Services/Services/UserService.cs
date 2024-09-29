using System;
using WebApp.Data;
using WebApp.Models;
using WebApp.Services.Interfaces;

namespace WebApp.Services.Services;

public class UserService : BaseService, IUserService
{
    public UserService(AppDbContext dbContext) : base(dbContext)
    {
    }

    public User AddUser(User user)
    {
        _dbContext.Users.Add(user);
        _dbContext.SaveChanges();
        return user;
    }

    public User DeleteUser(User user)
    {
        _dbContext.Users.Remove(user);
        _dbContext.SaveChanges();
        return user;
    }

    public User GetUser(int id)
    {
        var u = _dbContext.Users.FirstOrDefault(x => x.Id == id);
        return u!;
    }

    public IEnumerable<User> GetUsers()
    {
        var u = _dbContext.Users.AsEnumerable();
        return u;
    }

    public User UpdateUser(User user)
    {
        _dbContext.Users.Update(user);
        _dbContext.SaveChanges();
        return user;
    }
}
