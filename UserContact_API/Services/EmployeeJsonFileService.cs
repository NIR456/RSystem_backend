
using System.Text.Json;
using Employee_API.Models;

namespace Employee_API.Services
{
    public class EmployeeJsonFileService: IEmployeeJsonFileService
    {

        private readonly IWebHostEnvironment _webHostEnvironment;

        public EmployeeJsonFileService(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        private string JsonFileName
        {
            get { return Path.Combine(_webHostEnvironment.WebRootPath, "", "empoyees.json"); }
        }

        public async Task<Response> GetEmployeeList()
        {
            Response response = new Response();
            try
            {       
                if (!File.Exists(JsonFileName))
                {
                    await File.WriteAllTextAsync(JsonFileName, "[]");
                }
                string jsonData = await File.ReadAllTextAsync(JsonFileName);
                var users = JsonSerializer.Deserialize<EmployeeList[]>(jsonData, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                if (users != null)
                {
                    response.ResponseStatus = true;
                    response.ResponseObject = users;
                    response.ResponseMessage = "Success";
                }
                else
                {
                    response.ResponseStatus = true;
                    response.ResponseMessage = "Data Not found";
                }
            }
            catch (Exception ex)
            {
                response.ResponseStatus = false;
                response.ResponseMessage = ex.Message;
            }
            return response;
        }

        public async Task<Response> AddEmployee(EmployeeList newEmployee)
        {
            Response response = new();
            try
            {
                var currentResponse = await GetEmployeeList();
                    var contacts = currentResponse.ResponseObject as EmployeeList[];
                if (newEmployee.Id > 0)
                {
                    if (contacts != null) {
                        var contactIndex = contacts.ToList().FindIndex(c => c.Id == newEmployee.Id);
                        if (contactIndex >= 0)
                        {
                            contacts[contactIndex] = newEmployee;
                            await File.WriteAllTextAsync(JsonFileName, JsonSerializer.Serialize(contacts));
                            response.ResponseStatus = true;
                            response.ResponseMessage = "Contact updated successfully";
                        }
                        else
                        {
                            response.ResponseStatus = false;
                            response.ResponseMessage = "Contact not found";
                        }
                    }             
                }
                else
                {
                    
                    if (contacts != null)
                    {
                        int nextId = contacts.Any() ? contacts.Max(c => c.Id) + 1 : 1;
                        while (contacts.Any(c => c.Id == nextId))
                        {
                            nextId++;
                        }
                        newEmployee.Id = nextId;
                        var updatedContacts = contacts.Append(newEmployee).ToArray();
                        await File.WriteAllTextAsync(JsonFileName, JsonSerializer.Serialize(updatedContacts));

                        response.ResponseStatus = true;
                        response.ResponseMessage = "Contact added successfully";
                    }
                    else
                    {
                        response.ResponseStatus = false;
                        response.ResponseMessage = "Failed to retrieve contacts for adding";
                    }
                }
            }
            catch (Exception ex)
            {
                response.ResponseStatus = false;
                response.ResponseMessage = ex.Message;
            }
            return response;
        }

        public async Task<Response> RemoveEmployee(int Id)
        {
            Response response = new();
            try
            {
                var currentResponse = await GetEmployeeList();
                var contacts = currentResponse.ResponseObject as EmployeeList[];

                if (contacts != null)
                {
                    var contactIndex = contacts.ToList().FindIndex(c => c.Id == Id);
                    if (contactIndex >= 0)
                    {
                        var updatedContacts = contacts.ToList();
                        updatedContacts.RemoveAt(contactIndex);
                        await File.WriteAllTextAsync(JsonFileName, JsonSerializer.Serialize(updatedContacts.ToArray()));
                        response.ResponseStatus = true;
                        response.ResponseMessage = "Contact deleted successfully";
                    }
                    else
                    {
                        response.ResponseStatus = false;
                        response.ResponseMessage = "Contact not found";
                    }
                }
                else
                {
                    response.ResponseStatus = false;
                    response.ResponseMessage = "Failed to retrieve contacts for deletion";
                }
            }
            catch (Exception ex)
            {
                response.ResponseStatus = false;
                response.ResponseMessage = ex.Message;
            }
            return response;
        }

    }
}
