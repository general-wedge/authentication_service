using authentication_service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace authentication_service.Repositories
{
    public interface IRoleRepository : IGenericRepository<Role> { }

    public class RoleRepository : GenericRepository<Role>, IRoleRepository
    {
        private readonly user_auth_dbContext _context;

        public RoleRepository(user_auth_dbContext context)
            : base(context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
    }
}
