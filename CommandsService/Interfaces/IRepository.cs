using System.Linq.Expressions;
using CommandsService.Entities;

namespace CommandsService.Interfaces;

public interface IRepository<T> where T : EntityBase
{
    bool SaveChanges();

    IEnumerable<T> GetAll();
    
    IEnumerable<T> GetAll(Expression<Func<T, bool>> predicate);

    T? GetById(int id);

    void Create(T entity);

    bool Exists(int id);
}