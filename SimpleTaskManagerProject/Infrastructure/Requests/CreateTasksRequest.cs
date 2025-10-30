using DispatchR.Abstractions.Send;
using SimpleTaskManagerProject.Infrastructure.Dto;
using SimpleTaskManagerProject.Models;

namespace SimpleTaskManagerProject.Infrastructure.Requests;

public class CreateTaskRequest : IRequest<CreateTaskRequest, ValueTask<SimpleTask>>
{
    public required CreateSimpleTaskDto CreateTaskDto { get; init; }
}