using System.ComponentModel.DataAnnotations.Schema;

namespace SimpleTaskManagerProject.Models;

[Table("Tasks")]
public class SimpleTask
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public required string Description { get; set; }
}