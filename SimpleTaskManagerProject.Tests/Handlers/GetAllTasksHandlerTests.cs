using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using SimpleTaskManagerProject.Infrastructure;
using SimpleTaskManagerProject.Infrastructure.Handlers;
using SimpleTaskManagerProject.Infrastructure.Requests;
using SimpleTaskManagerProject.Models;

namespace SimpleTaskManagerProject.Tests.Handlers;

[TestFixture]
public class GetAllTasksHandlerTests
{
    [Test]
    public async Task ShouldReturnAllTasks()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: $"db_{Guid.NewGuid()}")
            .Options;
        await using var db = new AppDbContext(options);
        
        db.Tasks.AddRange(
            new SimpleTask { Title = "A", Description = "a" },
            new SimpleTask { Title = "B", Description = "b" }
        );
        await db.SaveChangesAsync();
        
        var handler = new GetAllTasksHandler(db);
        var result = await handler.Handle(new GetAllTasksRequest(), CancellationToken.None);
        
        result.Should().NotBeNull();
        result.Length.Should().Be(2);
        result.Select(t => t.Title).Should().BeEquivalentTo(new[] { "A", "B" });
    }
}
