using System.ComponentModel.DataAnnotations;

namespace CommandsService.Entities;

public class Platform : EntityBase
{    
    [Required]
    public int ExternalId { get; set; }

    [Required]
    public string? Name { get; set; }

    public ICollection<Command> Commands { get; set; } = new List<Command>();
}