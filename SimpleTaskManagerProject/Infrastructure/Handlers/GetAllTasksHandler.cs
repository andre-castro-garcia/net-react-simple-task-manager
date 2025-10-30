using DispatchR.Abstractions.Send;
using Microsoft.EntityFrameworkCore;
using SimpleTaskManagerProject.Infrastructure.Requests;
using SimpleTaskManagerProject.Models;

namespace SimpleTaskManagerProject.Infrastructure.Handlers;

public class GetAllTasksHandler(AppDbContext dbContext) : IRequestHandler<GetAllTasksRequest, ValueTask<SimpleTask[]>>
{
    public async ValueTask<SimpleTask[]> Handle(GetAllTasksRequest request, CancellationToken cancellationToken)
    {
        var tasks = await dbContext.Tasks.ToListAsync(cancellationToken: cancellationToken);
        return tasks.ToArray();
    }
}