using Application.interfaces;
using Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.interfaces
{
    public interface IComplaintRepository : IRepository<Complaint>
    {
        Task<List<Complaint>> GetByDepartmentAsync(int departmentId);
        Task<List<Complaint>> GetByUserAsync(int userId);
    }
}