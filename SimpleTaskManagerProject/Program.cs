using System.Reflection;
using DispatchR.Extensions;
using FluentValidation;
using Mapster;
using Microsoft.EntityFrameworkCore;
using OpenAI.Chat;
using SimpleTaskManagerProject.Api;
using SimpleTaskManagerProject.Hubs;
using SimpleTaskManagerProject.Infrastructure;
using SimpleTaskManagerProject.Infrastructure.Mapping;
using SimpleTaskManagerProject.Infrastructure.Validators;

namespace SimpleTaskManagerProject;

public class Program
{
    static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddCors(options =>
        {
            options.AddDefaultPolicy(
                policy =>
                {
                    policy
                        .AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
        });
        builder.Services.AddMapster();
        builder.Services.AddValidatorsFromAssemblyContaining<CreateSimpleTaskDtoValidator>();
        builder.Services.AddDispatchR(Assembly.GetExecutingAssembly());
        builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
        builder.Services.AddSignalR();
        builder.Services.AddSingleton<ChatClient>(serviceProvider =>
        {
            var apiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY");
            const string model = "gpt-5-nano";

            return new ChatClient(model, apiKey);
        });
        builder.Services.AddSingleton<SimpleTaskManagerProject.Infrastructure.Services.IChatService, SimpleTaskManagerProject.Infrastructure.Services.OpenAIChatService>();

        var app = builder.Build();
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        MappingConfig.Configure();
        
        app.RegisterRoutes();
        app.MapHub<TasksHub>("/tasks-hub");
        app.UseCors();
        app.Run();
    }
}