using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Microsoft.AspNetCore.Hosting;
using UserContact_API.Models;

namespace UserContact_API.Services
{
    public class ContactUserJsonFileService: IContactUserJsonFileService
    {

        private readonly IWebHostEnvironment _webHostEnvironment;

        public ContactUserJsonFileService(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        private string JsonFileName
        {
            get { return Path.Combine(_webHostEnvironment.WebRootPath, "", "contacts.json"); }
        }

        public async Task<Response> GetContactList()
        {
            Response response = new Response();
            try
            {       
                if (!File.Exists(JsonFileName))
                {
                    await File.WriteAllTextAsync(JsonFileName, "[]");
                }
                string jsonData = await File.ReadAllTextAsync(JsonFileName);
                var users = JsonSerializer.Deserialize<ContactList[]>(jsonData, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

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

        public async Task<Response> AddContact(ContactList newContact)
        {
            Response response = new();
            try
            {
                var currentResponse = await GetContactList();
                    var contacts = currentResponse.ResponseObject as ContactList[];
                if (newContact.Id > 0)
                {
                    if (contacts != null) {
                        var contactIndex = contacts.ToList().FindIndex(c => c.Id == newContact.Id);
                        if (contactIndex >= 0)
                        {
                            contacts[contactIndex] = newContact;
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
                        newContact.Id = nextId;
                        var updatedContacts = contacts.Append(newContact).ToArray();
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

        public async Task<Response> RemoveContact(int Id)
        {
            Response response = new();
            try
            {
                var currentResponse = await GetContactList();
                var contacts = currentResponse.ResponseObject as ContactList[];

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
