namespace PlatformService.Interfaces;

public interface IMessageBusClient<in T> where T : class 
{
    void Publish(T dto);
}