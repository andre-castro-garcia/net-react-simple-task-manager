using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using SimpleTaskManagerProject.Infrastructure.Dto;
using SimpleTaskManagerProject.Infrastructure.Requests;
using SimpleTaskManagerProject.Models;
using SimpleTaskManagerProject.Tests.Infrastructure;

namespace SimpleTaskManagerProject.Tests.Api;

[TestFixture]
public class TasksApiTests
{
    private TestWebApplicationFactory _factory = null!;

    [SetUp]
    public void SetUp()
    {
        _factory = new TestWebApplicationFactory();
    }

    [TearDown]
    public void TearDown()
    {
        _factory.Dispose();
    }

    [Test]
    public async Task ShouldReturn201WhenSuccessfulToCreateTask()
    {
        var client = _factory.CreateClientWithMocks();
        var dto = new CreateSimpleTaskDto { Title = "Task A", Description = "Do something" };
        
        var created = new SimpleTask { Id = 123, Title = dto.Title, Description = dto.Description };
        _factory.MediatorMock
            .Send(Arg.Any<CreateTaskRequest>(), Arg.Any<CancellationToken>())
            .Returns(new ValueTask<SimpleTask>(created));
        
        var response = await client.PostAsJsonAsync("/tasks", dto);
        
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var returned = await response.Content.ReadFromJsonAsync<SimpleTask>();
        returned.Should().NotBeNull();
        returned!.Id.Should().Be(123);

        await _factory.MediatorMock
            .Received(1)
            .Send(Arg.Is<CreateTaskRequest>(r => r.CreateTaskDto.Title == dto.Title), Arg.Any<CancellationToken>());
    }

    [Test]
    public async Task ShouldReturn400WhenCreateTaskValidationFails()
    {
        var client = _factory.CreateClientWithMocks();
        var dto = new CreateSimpleTaskDto { Title = "", Description = "X" };
        var response = await client.PostAsJsonAsync("/tasks", dto);
        
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        await _factory.MediatorMock.DidNotReceiveWithAnyArgs()
            .Send<CreateTaskRequest, ValueTask<SimpleTask>>(default!, default);
    }

    [Test]
    public async Task ShouldReturnOkWithTasksWhenCalled()
    {
        var client = _factory.CreateClientWithMocks();
        var list = new[]
        {
            new SimpleTask { Id = 1, Title = "A", Description = "a" },
            new SimpleTask { Id = 2, Title = "B", Description = "b" }
        };

        _factory.MediatorMock
            .Send(Arg.Any<GetAllTasksRequest>(), Arg.Any<CancellationToken>())
            .Returns(new ValueTask<SimpleTask[]>(list));
        
        var response = await client.GetAsync("/tasks");
        response.EnsureSuccessStatusCode();
        
        var returned = await response.Content.ReadFromJsonAsync<SimpleTask[]>();
        
        returned.Should().NotBeNull();
        returned!.Length.Should().Be(2);
        await _factory.MediatorMock
            .Received(1)
            .Send(Arg.Any<GetAllTasksRequest>(), Arg.Any<CancellationToken>());
    }
}