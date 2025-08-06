using Application.interfaces;
using Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Persistance.Context;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Persistance.Repositories
{
    public class ComplaintRepository : Repository<Complaint>, IComplaintRepository
    {
        private readonly BiCozumContext _context;

        public ComplaintRepository(BiCozumContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<Complaint>> GetByDepartmentAsync(int departmentId)
        {
            return await _context.Complaints
                .Where(c => c.DepartmentId == departmentId)
                .ToListAsync();
        }

        public async Task<List<Complaint>> GetByUserAsync(int userId)
        {
            return await _context.Complaints
                .Where(c => c.UserId == userId)
                .ToListAsync();
        }

        public async Task<List<Complaint>> GetAllWithIncludesAsync()
        {
            return await _context.Complaints
                .Include(c => c.Department)
                .Include(c => c.User)
                .ToListAsync();
        }

        public async Task<Complaint?> GetByIdWithIncludesAsync(int id)
        {
            return await _context.Complaints
                .Include(c => c.Department)
                .Include(c => c.User)
                .FirstOrDefaultAsync(c => c.Id == id);
        }
    }
}