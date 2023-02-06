namespace CommandsService.Interfaces;

public interface IEventProcessor
{
    void ProcessEvent(string message);
}