using Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        Task<List<User>> GetUsersByDepartmentAsync(int departmentId);
        Task<User?> GetByEmailAsync(string email);
        Task<List<User>> GetAllWithIncludesAsync();
        Task<User?> GetByIdWithIncludesAsync(int id);
    }
}
