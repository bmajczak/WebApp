using System;
using WebApp.Data;

namespace WebApp.Services.Services;

public abstract class BaseService
{
    protected readonly AppDbContext _dbContext;

    public BaseService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
}