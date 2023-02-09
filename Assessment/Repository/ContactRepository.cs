using Assessment.Context;
using Assessment.Interfaces;
using Assessment.Models;
using Assessment.Models.Requests;
using Assessment.Models.Response;
using Microsoft.EntityFrameworkCore;

namespace Assessment.Repository
{
    public class ContactRepository : IContactRepository
    {
        private readonly ContactsContext _contactDbContext;

        public ContactRepository(ContactsContext contactDbContext)
        {
            _contactDbContext = contactDbContext;
        }

        public async Task AddContact(ContactRequest contactRequest)
        {
            _contactDbContext.Contacts.Add(new Contact
            {
                Id = Guid.NewGuid(),
                Name = contactRequest.Name,
                Address = contactRequest.Address,
            });

            await _contactDbContext.SaveChangesAsync();
        }

        public async Task DeleteContact(Guid id)
        {
            var contact = await _contactDbContext.Contacts.SingleOrDefaultAsync(x => x.Id == id);

            if (contact is not null)
            { 
                _contactDbContext.Contacts.Remove(contact);

                await _contactDbContext.SaveChangesAsync();
            }
        }

        public async Task<bool> Exists(string name)
        {
            var contact = await _contactDbContext.Contacts.SingleOrDefaultAsync(x => x.Name.ToLower() == name.ToLower());

            return contact != null;
        }

        public async Task<IEnumerable<ContactResponse>> GetContacts()
        {
            var contacts =  await _contactDbContext.Contacts.ToListAsync();

            return contacts.Select(x => new ContactResponse
            {
                Id = x.Id,
                Address = x.Address,
                Name = x.Name,
            });
        }

        public async Task UpdateContact(ContactUpdate contactUpdate, Guid id)
        {
           var contact = await _contactDbContext.Contacts.SingleOrDefaultAsync(x => x.Id == id);

            if(contact is not null)
            {
                contact.Name = contactUpdate.Name;
                contact.Address = contactUpdate.Address;

                _contactDbContext.Contacts.Update(contact);

                await _contactDbContext.SaveChangesAsync();
            } 
        }
    }
}
