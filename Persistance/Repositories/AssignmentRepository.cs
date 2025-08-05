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
    public class AssignmentRepository : Repository<Assignment>, IAssignmentRepository
    {
        private readonly BiCozumContext _context;

        public AssignmentRepository(BiCozumContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<Assignment>> GetByComplaintAsync(int complaintId)
        {
            return await _context.Assignments
                .Where(a => a.ComplaintId == complaintId)
                .ToListAsync();
        }

        public async Task<List<Assignment>> GetByUserAsync(int userId)
        {
            return await _context.Assignments
                .Where(a => a.UserId == userId)
                .ToListAsync();
        }
    }
}
