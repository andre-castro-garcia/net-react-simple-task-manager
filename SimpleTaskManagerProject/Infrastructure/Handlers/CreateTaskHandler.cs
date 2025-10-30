using DispatchR.Abstractions.Send;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using SimpleTaskManagerProject.Infrastructure.Requests;
using SimpleTaskManagerProject.Models;

namespace SimpleTaskManagerProject.Infrastructure.Handlers;

public class CreateTaskHandler(AppDbContext dbContext, IMapper mapper) : 
    IRequestHandler<CreateTaskRequest, ValueTask<SimpleTask>>
{
    public async ValueTask<SimpleTask> Handle(CreateTaskRequest request, CancellationToken cancellationToken)
    {
        var task = mapper.Map<SimpleTask>(request.CreateTaskDto);
        
        dbContext.Add(task);
        await dbContext.SaveChangesAsync(cancellationToken);
        
        return task;
    }
}