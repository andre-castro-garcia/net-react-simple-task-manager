using DispatchR.Abstractions.Send;
using SimpleTaskManagerProject.Models;

namespace SimpleTaskManagerProject.Infrastructure.Requests;

public class GetAllTasksRequest: IRequest<GetAllTasksRequest, ValueTask<SimpleTask[]>>;