using System.Reflection;
using DispatchR.Extensions;
using FluentValidation;
using Mapster;
using Microsoft.EntityFrameworkCore;
using SimpleTaskManagerProject.Api;
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
                    policy.AllowAnyOrigin();
                });
        });
        builder.Services.AddMapster();
        builder.Services.AddValidatorsFromAssemblyContaining<CreateSimpleTaskDtoValidator>();
        builder.Services.AddDispatchR(Assembly.GetExecutingAssembly());
        builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));


        var app = builder.Build();
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        MappingConfig.Configure();
        
        app.RegisterRoutes();
        app.UseCors();
        app.Run();
    }
}