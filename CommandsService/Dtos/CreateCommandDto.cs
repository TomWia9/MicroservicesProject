namespace CommandsService.Dtos;

public class CreateCommandDto
{
    public string? HowTo { get; set; }

    public string? CommandLine { get; set; }

    public int PlatformId { get; set; }
}