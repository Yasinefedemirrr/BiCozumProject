using Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.interfaces
{
    public interface IComplaintHistoryRepository : IRepository<ComplaintHistory>
    {
        Task<List<ComplaintHistory>> GetByComplaintAsync(int complaintId);
        Task<List<ComplaintHistory>> GetAllWithIncludesAsync();
        Task<ComplaintHistory?> GetByIdWithIncludesAsync(int id);
    }
}
