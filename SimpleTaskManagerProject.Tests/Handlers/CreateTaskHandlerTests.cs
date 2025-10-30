using FluentAssertions;
using MapsterMapper;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using NUnit.Framework;
using SimpleTaskManagerProject.Hubs;
using SimpleTaskManagerProject.Infrastructure;
using SimpleTaskManagerProject.Infrastructure.Handlers;
using SimpleTaskManagerProject.Infrastructure.Dto;
using SimpleTaskManagerProject.Infrastructure.Requests;
using SimpleTaskManagerProject.Models;

namespace SimpleTaskManagerProject.Tests.Handlers;

[TestFixture]
public class CreateTaskHandlerTests
{
    [Test]
    public async Task ShouldCreateAndPersistTask()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: $"db_{Guid.NewGuid()}")
            .Options;
        await using var db = new AppDbContext(options);
        
        var mapper = Substitute.For<IMapper>();
        
        var hub = Substitute.For<IHubContext<TasksHub>>();
        var clients = Substitute.For<IHubClients>();
        var clientProxy = Substitute.For<IClientProxy>();
        hub.Clients.Returns(clients);
        clients.All.Returns(clientProxy);
        
        var dto = new CreateSimpleTaskDto { Title = "T1", Description = "D1" };
        mapper.Map<SimpleTask>(dto).Returns(new SimpleTask { Title = dto.Title, Description = dto.Description });
        
        var handler = new CreateTaskHandler(db, mapper, hub);
        var result = await handler.Handle(new CreateTaskRequest { CreateTaskDto = dto }, CancellationToken.None);
        result.Id.Should().BeGreaterThan(0);
        
        var saved = await db.Tasks.FirstOrDefaultAsync(t => t.Id == result.Id);
        saved.Should().NotBeNull();
        saved!.Title.Should().Be("T1");
        saved.Description.Should().Be("D1");
        
        await clientProxy.Received(1).SendCoreAsync(
            "taskCreated",
            Arg.Is<object[]>(a => a.Length == 1 && a[0] is SimpleTask),
            Arg.Any<CancellationToken>());
    }
}
