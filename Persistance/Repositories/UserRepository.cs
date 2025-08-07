using Application.interfaces;
using Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Persistance.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistance.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        private readonly BiCozumContext _context;

        public UserRepository(BiCozumContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<User>> GetUsersByDepartmentAsync(int departmentId)
        {
            return await _context.Users
                .Where(u => u.DepartmentId == departmentId)
                .Include(u => u.Department)
                .ToListAsync();
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Users
                .Include(u => u.Department)
                .FirstOrDefaultAsync(u => u.Username == email); // Username'e göre kontrol ettim
        }

        public async Task<List<User>> GetAllWithIncludesAsync()
        {
            return await _context.Users
                .Include(u => u.Department)
                .Include(u => u.AppRole)
                .ToListAsync();
        }

        public async Task<User?> GetByIdWithIncludesAsync(int id)
        {
            return await _context.Users
                .Include(u => u.Department)
                .Include(u => u.AppRole)
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<AppRole?> GetRoleByNameAsync(string roleName)
        {
            return await _context.AppRoles.FirstOrDefaultAsync(r => r.AppRoleName == roleName);
        }
    }

}
