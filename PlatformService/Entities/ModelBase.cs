using System.ComponentModel.DataAnnotations;

namespace PlatformService.Entities;

public abstract class EntityBase
{
    [Key]
    [Required]
    public int Id { get; set; }
}