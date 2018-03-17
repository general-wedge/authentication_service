using authentication_service.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace authentication_service.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IEnumerable<T>> Get();
        Task<IEnumerable<T>> Get(Expression<Func<T, bool>> predicate);
        Task<T> Get(int id);
        Task<T> Add(T entity);
        Task<T> Delete(T entity);
        Task<T> Update(T entity);
    }

    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly user_auth_dbContext _context;

        public GenericRepository(user_auth_dbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<T> Add(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            await _context.SaveChangesAsync();
            return await Task.FromResult(entity);
        }

        public async Task<T> Delete(T entity)
        {
            T existing = await _context.Set<T>().FindAsync(entity);
            if (existing != null)
            {
                _context.Set<T>().Remove(existing);
                await _context.SaveChangesAsync();
            }
            return await Task.FromResult(entity);
        }

        public async Task<IEnumerable<T>> Get()
        {
            return await Task.FromResult(_context.Set<T>().AsEnumerable<T>());
        }

        public async Task<IEnumerable<T>> Get(Expression<Func<T, bool>> predicate)
        {
            return await Task.FromResult(_context.Set<T>().Where(predicate).AsEnumerable<T>());
        }

        public async Task<T> Get(int id)
        {
            return await Task.FromResult(_context.Set<T>().Find(id));
        }

        public async Task<T> Update(T entity)
        {
            _context.Set<T>().Update(entity);
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return await Task.FromResult(entity);
        }

    }
}
