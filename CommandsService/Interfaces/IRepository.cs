using CommandsService.Entities;

namespace CommandsService.Interfaces;

public interface IRepository<T> where T : EntityBase
{
    bool SaveChanges();

    IEnumerable<T> GetAll();

    T? GetById(int id);

    void Create(T entity);
}