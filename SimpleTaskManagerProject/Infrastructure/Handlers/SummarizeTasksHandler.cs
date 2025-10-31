using System.Text;
using DispatchR.Abstractions.Send;
using Microsoft.EntityFrameworkCore;
using SimpleTaskManagerProject.Infrastructure.Requests;
using SimpleTaskManagerProject.Infrastructure.Services;

namespace SimpleTaskManagerProject.Infrastructure.Handlers;

public class SummarizeTasksHandler(AppDbContext context, IChatService chat) : IRequestHandler<SummarizeTasksRequest, ValueTask<string>>
{
    public async ValueTask<string> Handle(SummarizeTasksRequest request, CancellationToken cancellationToken)
    {
        var instructions = new StringBuilder(@"
                    Below are a task list. The title is in parentehesis and the rest is the task description.
                    Your work is to write a summary based on the tasks.
                    
                    Tasks:
                ");
        var tasks = await context.Tasks.ToListAsync(cancellationToken);
        tasks.ForEach(t =>
        {
            instructions.AppendLine($"({t.Title}){t.Description}");
        });
                
        var summary = await chat.SummarizeAsync(instructions.ToString(), cancellationToken);
        return summary;
    }
}