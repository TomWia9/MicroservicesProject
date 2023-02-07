using CommandsService.Entities;

namespace CommandsService.Interfaces;

public interface IDataClient<out T> where T: EntityBase
{
    IEnumerable<T>? ReturnAll();
}