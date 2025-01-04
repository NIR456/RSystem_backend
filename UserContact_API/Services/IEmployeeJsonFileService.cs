using Employee_API.Models;

namespace Employee_API.Services
{
    public interface IEmployeeJsonFileService
    {
        public Task<Response> GetEmployeeList();
        public Task<Response> AddEmployee(EmployeeList EmployeeList);
        public Task<Response> RemoveEmployee(int Id);
    }
}
