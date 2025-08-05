using Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.interfaces
{
    public interface IAssignmentRepository : IRepository<Assignment>
    {
        Task<List<Assignment>> GetByComplaintAsync(int complaintId);
        Task<List<Assignment>> GetByUserAsync(int userId);
    }
}
