namespace SimpleTaskManagerProject.Infrastructure.Services;

public interface IChatService
{
    Task<string> SummarizeAsync(string instructions, CancellationToken cancellationToken);
}
