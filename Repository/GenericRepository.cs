using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BusinessObjects;


namespace Repository
{
    public class GenericRepository<T> : IRepository<T> where T : class
    {
        private readonly FuminiHotelManagementContext _context;
        private readonly DbSet<T> _dbSet;
        
        

        public GenericRepository()
        {
            _context = new FuminiHotelManagementContext(AppSettingsReader.GetString("ConnectionStrings", "DefaultConnection"));
            
            _dbSet = _context.Set<T>();
        }

        public IEnumerable<T> GetAll() => _dbSet.ToList();
        public T GetById(object id) => _dbSet.Find(id);
       
        public IEnumerable<T> Find(Expression<Func<T, bool>> predicate) => _dbSet.Where(predicate).ToList();
        public void Add(T entity) { 
            _context.ChangeTracker.Clear();
            _dbSet.Add(entity);
        }
        public void Update(T entity)
        {
            var tracked = _context.ChangeTracker.Entries<T>()
                .FirstOrDefault(e => e.Property("RoomId").CurrentValue.Equals(_context.Entry(entity).Property("RoomId").CurrentValue));
            if (tracked != null)
                tracked.State = EntityState.Detached;
            _dbSet.Update(entity);
        }
        public void Delete(object id)
        {
            var entity = GetById(id);
            if (entity != null) _dbSet.Remove(entity);
        }
        public void Save() => _context.SaveChanges();
    }
}
