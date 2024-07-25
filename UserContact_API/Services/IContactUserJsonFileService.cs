using UserContact_API.Models;

namespace UserContact_API.Services
{
    public interface IContactUserJsonFileService
    {
        public Task<Response> GetContactList();
        public Task<Response> AddContact(ContactList contactList);
        public Task<Response> RemoveContact(int Id);
    }
}
