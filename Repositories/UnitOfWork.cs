using authentication_service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace authentication_service.Repositories
{
    public interface IUnitOfWork
    {
        user_auth_dbContext Context { get; }
        void Commit();
    }

    public class UnitOfWork : IUnitOfWork
    {
        public user_auth_dbContext Context { get; }

        public UnitOfWork(user_auth_dbContext context)
        {
            Context = context;
        }

        public void Commit()
        {
            Context.SaveChangesAsync();
        }

        public void Dispose()
        {
            Context.Dispose();
        }
    }
}
