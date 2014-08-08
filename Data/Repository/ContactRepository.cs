using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DomainModel;
using Core.Interface.Repository;
using Data.Context;

namespace Data.Repository
{
    public class ContactRepository : EfRepository<Contact>, IContactRepository
    {

        private StockControlEntities entities;
        public ContactRepository()
        {
            entities = new StockControlEntities();
        }

        public IList<Contact> GetAll()
        {
            return FindAll().ToList();
        }

        public Contact GetObjectById(int Id)
        {
            Contact contact = Find(x => x.Id == Id && !x.IsDeleted);
            if (contact != null) { contact.Errors = new Dictionary<string, string>(); }
            return contact;
        }

        public Contact CreateObject(Contact contact)
        {
            contact.IsDeleted = false;
            contact.CreatedAt = DateTime.Now;
            return Create(contact);
        }

        public Contact UpdateObject(Contact contact)
        {
            contact.UpdatedAt = DateTime.Now;
            Update(contact);
            return contact;
        }

        public Contact SoftDeleteObject(Contact contact)
        {
            contact.IsDeleted = true;
            contact.DeletedAt = DateTime.Now;
            Update(contact);
            return contact;
        }

        public bool DeleteObject(int Id)
        {
            Contact contact = Find(x => x.Id == Id);
            return (Delete(contact) == 1) ? true : false;
        }
    }
}
