using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DomainModel;
using Core.Interface.Repository;
using Core.Interface.Service;
using Core.Interface.Validation;

namespace Service.Service
{
    public class ContactService : IContactService
    {
        private IContactRepository _repository;
        private IContactValidator _validator;
        public ContactService(IContactRepository _contactRepository, IContactValidator _contactValidator)
        {
            _repository = _contactRepository;
            _validator = _contactValidator;
        }

        public IContactValidator GetValidator()
        {
            return _validator;
        }

        public IList<Contact> GetAll()
        {
            return _repository.GetAll();
        }

        public Contact GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public Contact GetObjectByName(string Name)
        {
            return _repository.FindAll(i => i.Name == Name && !i.IsDeleted).FirstOrDefault();
        }

        public Contact CreateObject(Contact contact)
        {
            contact.Errors = new Dictionary<String, String>();
            return (_validator.ValidCreateObject(contact) ? _repository.CreateObject(contact) : contact);
        }

        public Contact UpdateObject(Contact contact)
        {
            return (contact = _validator.ValidUpdateObject(contact) ? _repository.UpdateObject(contact) : contact);
        }

        public Contact SoftDeleteObject(Contact contact, IPurchaseOrderService _purchaseOrderService, ISalesOrderService _salesOrderService)
        {
            return (contact = _validator.ValidDeleteObject(contact, _purchaseOrderService, _salesOrderService) ? _repository.SoftDeleteObject(contact) : contact);
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }

    }
}
