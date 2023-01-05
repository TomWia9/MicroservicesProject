using System.Linq.Expressions;
using CommandsService.Entities;
using CommandsService.Interfaces;

namespace CommandsService.Data;

public class Repository<T> : IRepository<T> where T : EntityBase
{
    private readonly AppDbContext _context;

    public Repository(AppDbContext context)
    {
        _context = context;
    }

    public void Create(T entity)
    {
        if(entity == null)
        {
            throw new ArgumentNullException(nameof(entity));
        }

        _context.Set<T>().Add(entity);
    }

    public bool Exists(int id)
    {
        return _context.Set<T>().Any();
    }

    public IEnumerable<T> GetAll()
    {
        return _context.Set<T>().ToList();
    }

    public IEnumerable<T> GetAll(Expression<Func<T, bool>> predicate)
    {
        return _context.Set<T>().Where(predicate).ToList();
    }

    public T? GetById(int id)
    {
        return _context.Set<T>().Find(id);
    }

    public bool SaveChanges()
    {
        return _context.SaveChanges() >= 0;
    }
}