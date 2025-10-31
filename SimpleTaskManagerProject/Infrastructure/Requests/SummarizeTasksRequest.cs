using DispatchR.Abstractions.Send;

namespace SimpleTaskManagerProject.Infrastructure.Requests;

public class SummarizeTasksRequest: IRequest<SummarizeTasksRequest, ValueTask<string>>;