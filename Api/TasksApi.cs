using FluentValidation;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using SimpleTaskManagerProject.Infrastructure;
using SimpleTaskManagerProject.Infrastructure.Dto;
using SimpleTaskManagerProject.Models;

namespace SimpleTaskManagerProject.Api;

public static class TasksApi
{
    public static void RegisterRoutes(this WebApplication app)
    {
        app.MapPost("/tasks", async (AppDbContext dbContext, IMapper mapper, 
                IValidator<CreateSimpleTaskDto> validator, CreateSimpleTaskDto dto) =>
            {
                var result = await validator.ValidateAsync(dto);
                if (!result.IsValid)
                {
                    // Add log infra to register the request errors
                    return Results.BadRequest();
                }
                
                var task = mapper.Map<SimpleTask>(dto);
                
                dbContext.Add(task);
                await dbContext.SaveChangesAsync();
                return Results.Created($"/tasks/{task.Id}", task);
            })
            .WithName("CreateTask")
            .WithOpenApi();
        
        app.MapGet("/tasks", async (AppDbContext db) =>
            {
                /* For tests purposes I will not include pagination here, for prod-ready
                 applications we should add more features here */
                var tasks = await db.Tasks.ToListAsync();
                return tasks;
            })
            .WithName("GetTasks")
            .WithOpenApi();
    }
}