using System.ComponentModel.DataAnnotations;

namespace CommandsService.Entities;

public abstract class EntityBase
{
    [Key]
    [Required]
    public int Id { get; set; }
}