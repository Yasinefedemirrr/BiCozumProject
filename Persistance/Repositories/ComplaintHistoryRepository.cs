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
    public class ComplaintHistoryRepository : Repository<ComplaintHistory>, IComplaintHistoryRepository
    {
        private readonly BiCozumContext _context;

        public ComplaintHistoryRepository(BiCozumContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<ComplaintHistory>> GetByComplaintAsync(int complaintId)
        {
            return await _context.ComplaintHistories
                .Include(ch => ch.Complaint)
                .Where(ch => ch.ComplaintId == complaintId)
                .ToListAsync();
        }

        public async Task<List<ComplaintHistory>> GetAllWithIncludesAsync()
        {
            return await _context.ComplaintHistories
                .Include(ch => ch.Complaint)
                .ToListAsync();
        }

        public async Task<ComplaintHistory?> GetByIdWithIncludesAsync(int id)
        {
            return await _context.ComplaintHistories
                .Include(ch => ch.Complaint)
                .FirstOrDefaultAsync(ch => ch.Id == id);
        }
    }
}