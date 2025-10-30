using Mapster;
using SimpleTaskManagerProject.Infrastructure.Dto;
using SimpleTaskManagerProject.Models;

namespace SimpleTaskManagerProject.Infrastructure.Mapping;

public static class MappingConfig
{
    public static void Configure()
    {
        TypeAdapterConfig<CreateSimpleTaskDto, SimpleTask>.NewConfig();
    }
}