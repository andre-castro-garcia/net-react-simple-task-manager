using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using NUnit.Framework;
using SimpleTaskManagerProject.Infrastructure;
using SimpleTaskManagerProject.Infrastructure.Handlers;
using SimpleTaskManagerProject.Infrastructure.Requests;
using SimpleTaskManagerProject.Infrastructure.Services;
using SimpleTaskManagerProject.Models;

namespace SimpleTaskManagerProject.Tests.Handlers;

[TestFixture]
public class SummarizeTasksHandlerTests
{
    [Test]
    public async Task ShouldReturnSummary()
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

        var chat = Substitute.For<IChatService>();
        chat.SummarizeAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromResult("summary text"));

        var handler = new SummarizeTasksHandler(db, chat);
        var result = await handler.Handle(new SummarizeTasksRequest(), CancellationToken.None);

        result.Should().Be("summary text");
        await chat.Received(1).SummarizeAsync(Arg.Is<string>(s => s.Contains("Tasks:")), Arg.Any<CancellationToken>());
    }
}
