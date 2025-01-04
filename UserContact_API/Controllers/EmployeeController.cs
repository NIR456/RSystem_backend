
using Employee_API.Services;
using Microsoft.AspNetCore.Mvc;
using Employee_API.Models;

namespace Employee_API.Controllers
{
    [ApiController]
    [Route("api_employee/[controller]/[action]")]
    public class EmployeeController : Controller
    {
        private readonly IEmployeeJsonFileService _EmployeeService;

        public EmployeeController(IEmployeeJsonFileService EmployeeService)
        {
            _EmployeeService = EmployeeService;
        }

        [HttpGet]
        public  async Task<Response> GetEmployeeListData()
        {
            return await _EmployeeService.GetEmployeeList();
        }

        [HttpPost]
        public async Task<Response> AddOrUpdateEmployeeList([FromBody] EmployeeList newEmployee)
        {
            return await _EmployeeService.AddEmployee(newEmployee);
        }

        [HttpGet]
        public async Task<Response> RemoveEmployeeList(int Id)
        {
            return await _EmployeeService.RemoveEmployee(Id);
        }
    }
}
