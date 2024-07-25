
using Microsoft.AspNetCore.Mvc;
using UserContact_API.Models;
using UserContact_API.Services;

namespace UserContact_API.Controllers
{
    [ApiController]
    [Route("api_contact/[controller]/[action]")]
    public class ContactController : Controller
    {
        private readonly IContactUserJsonFileService _ContactService;

        public ContactController(IContactUserJsonFileService ContactService)
        {
            _ContactService = ContactService;
        }

        [HttpGet]
        public  async Task<Response> GetContactListData()
        {
            return await _ContactService.GetContactList();
        }

        [HttpPost]
        public async Task<Response> AddOrUpdateContactList([FromBody] ContactList newContact)
        {
            return await _ContactService.AddContact(newContact);
        }

        [HttpGet]
        public async Task<Response> RemoveContactList(int Id)
        {
            return await _ContactService.RemoveContact(Id);
        }
    }
}
