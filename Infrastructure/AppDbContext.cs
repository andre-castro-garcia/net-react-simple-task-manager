using Microsoft.EntityFrameworkCore;
using SimpleTaskManagerProject.Models;

namespace SimpleTaskManagerProject.Infrastructure;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<SimpleTask> Tasks { get; set; }
}