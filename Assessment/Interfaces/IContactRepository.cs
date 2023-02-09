using Assessment.Models.Requests;
using Assessment.Models.Response;

namespace Assessment.Interfaces
{
    public interface IContactRepository
    {
        Task<bool> Exists(string name);

        Task<IEnumerable<ContactResponse>> GetContacts();

        Task AddContact(ContactRequest contactRequest);

        Task UpdateContact(ContactUpdate contactUpdate, Guid id);

        Task DeleteContact(Guid id);
    }
}
