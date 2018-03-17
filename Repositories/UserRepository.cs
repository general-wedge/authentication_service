using authentication_service.Models;
using authentication_service.Utilities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace authentication_service.Repositories
{

    public interface IUserRepository : IDisposable
    {
        IEnumerable<User> GetAll();
        Task<User> GetByIdAsync(int id);
        Task<User> GetByUsernameAsync(string username);
        Task<User> CreateUserAsync(User user);
        Task<User> UpdateUserAsync(int id, User user);
        Task<User> DeleteUserAsync(int id);
        Task<bool> UserExists(int id);
        Task<bool> UserEmailExists(string email);
        Task<bool> UsernameExists(string username);
    }

    public class UserRepository : IUserRepository
    {
        private user_auth_dbContext _context;

        public UserRepository(user_auth_dbContext context)
        {
            this._context = context ?? throw new ArgumentNullException(nameof(context)); // Guard to ensure db context isn't null
        }

        public async Task<User> CreateUserAsync(User user)
        {
            string salt = Encryption.CreateSalt(32);
            string pwHash = Encryption.GenerateSHA256Hash(user.Password, salt);
            user.Salt = salt;
            user.Password = pwHash;
            user.DateCreated = DateTime.UtcNow;
            _context.User.Add(user);
            _context.SaveChanges();
            //var createdUser = await _context.User.AsNoTracking().Where(x => x.Id == user.Id).FirstOrDefaultAsync();
            return await Task.FromResult(user);
        }

        public Task<User> DeleteUserAsync(int id)
        {
            throw new NotSupportedException();
        }

        public IEnumerable<User> GetAll()
        {
            return _context.User.Include(x => x.AcctState).Include(y => y.UserGroup).ThenInclude(z => z.Group)
                .Include(y => y.UserPermission).ThenInclude(z => z.Permission).AsEnumerable();
        }

        public async Task<User> GetByIdAsync(int id)
        {
            return await Task.FromResult(_context.User.Include(x => x.AcctState).Include(y => y.UserGroup).ThenInclude(z => z.Group)
                .Include(y => y.UserPermission).ThenInclude(z => z.Permission).Single(x => x.Id == id));
        }

        public async Task<User> GetByUsernameAsync(string username)
        {
            return await Task.FromResult(_context.User.Include(x => x.AcctState).Include(y => y.UserGroup).ThenInclude(z => z.Group)
                .Include(y => y.UserPermission).ThenInclude(z => z.Permission).Single(x => x.Username == username));
        }

        public async Task<User> UpdateUserAsync(int id, User user)
        {
            user.DateUpdated = DateTime.UtcNow;
            _context.User.Update(user);
            _context.Entry(user).State = EntityState.Modified;
            _context.SaveChanges();
            return await Task.FromResult(user);
        }

        public async Task<bool> UserExists(int userId)
        {
            return await Task.FromResult(_context.User.Any(x => x.Id == userId));
        }

        public async Task<bool> UserEmailExists(string email)
        {
            return await Task.FromResult(_context.User.Any(x => x.Email == email));
        }

        public async Task<bool> UsernameExists(string username)
        {
            return await Task.FromResult(_context.User.Any(x => x.Username == username));
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~UserRepository() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}