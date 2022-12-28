using System.ComponentModel.DataAnnotations;

namespace PlatformService.Entities;

public class Platform : EntityBase
{    
    [Required]
    public string Name { get; set; }

    [Required]
    public string Publisher { get; set; }

    [Required]
    public string Cost { get; set; }
    
}