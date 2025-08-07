using Application.interfaces;
using Domain.Entity;
using Persistance.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Persistance.Repositories
{
    public class AppUserRepository : IAppUserRepository
    {
        private readonly BiCozumContext _context;
        public AppUserRepository(BiCozumContext context)
        {
            _context = context;
        }
        public Task<List<User>> GetByFilterAsync(Expression<Func<User, bool>> filter)
        {
            throw new NotImplementedException();
        }
    }
}
