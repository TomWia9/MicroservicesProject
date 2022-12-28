using PlatformService.Entities;
using PlatformService.Interfaces;

namespace PlatformService.Data;

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
            throw new ArgumentNullException(nameof(T));
        }

        _context.Set<T>().Add(entity);
    }

    public IEnumerable<T> GetAll()
    {
        return _context.Set<T>().ToList();
    }

    public T GetById(int id)
    {
        return _context.Set<T>().Find(id);
    }

    public bool SaveChanges()
    {
        return _context.SaveChanges() >= 0;
    }
}