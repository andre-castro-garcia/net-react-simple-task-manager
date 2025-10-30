using DispatchR.Abstractions.Send;
using MapsterMapper;
using Microsoft.AspNetCore.SignalR;
using SimpleTaskManagerProject.Hubs;
using SimpleTaskManagerProject.Infrastructure.Requests;
using SimpleTaskManagerProject.Models;

namespace SimpleTaskManagerProject.Infrastructure.Handlers;

public class CreateTaskHandler(AppDbContext dbContext, IMapper mapper, IHubContext<TasksHub> hub) : 
    IRequestHandler<CreateTaskRequest, ValueTask<SimpleTask>>
{
    public async ValueTask<SimpleTask> Handle(CreateTaskRequest request, CancellationToken cancellationToken)
    {
        var task = mapper.Map<SimpleTask>(request.CreateTaskDto);
        dbContext.Add(task);
        
        await dbContext.SaveChangesAsync(cancellationToken);
        await NotifyConnectedClients(task);
        
        return task;
    }

    private async Task NotifyConnectedClients(SimpleTask task)
    {
        await hub.Clients.All.SendAsync("taskCreated", task);
    }
}