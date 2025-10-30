using DispatchR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NSubstitute;

namespace SimpleTaskManagerProject.Tests.Infrastructure;

public class TestWebApplicationFactory : WebApplicationFactory<Program>
{
    public IMediator MediatorMock { get; } = Substitute.For<IMediator>();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            services.RemoveAll<IMediator>();
            // Remove DbContext to avoid touching the SQLite database
            services.RemoveAll<DbContext>();
            services.AddSingleton(MediatorMock);
        });
    }

    public HttpClient CreateClientWithMocks()
    {
        return CreateClient(new WebApplicationFactoryClientOptions());
    }
}