using DispatchR;
using FluentValidation;
using SimpleTaskManagerProject.Infrastructure.Dto;
using SimpleTaskManagerProject.Infrastructure.Requests;

namespace SimpleTaskManagerProject.Api;

public static class TasksApi
{
    public static void RegisterRoutes(this WebApplication app)
    {
        app.MapPost("/tasks", async (IMediator mediatr,
                IValidator<CreateSimpleTaskDto> validator, CreateSimpleTaskDto dto) =>
            {
                var result = await validator.ValidateAsync(dto);
                if (!result.IsValid)
                {
                    // Add log infra to register the request errors
                    return Results.BadRequest();
                }

                var task = await mediatr.Send(new CreateTaskRequest()
                {
                    CreateTaskDto = dto
                }, CancellationToken.None);
                return Results.Created($"/tasks/{task.Id}", task);
            })
            .WithName("CreateTask")
            .WithOpenApi();
        
        app.MapGet("/tasks", async (IMediator mediatr) =>
            {
                /* For tests purposes I will not include pagination here, for prod-ready
                 applications we should add more features here */
                var tasks = await mediatr.Send(new GetAllTasksRequest(), CancellationToken.None);
                return tasks;
            })
            .WithName("GetTasks")
            .WithOpenApi();
    }
}