using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DomainModel;
using Core.Interface.Validation;

namespace Core.Interface.Service
{
    public interface IContactService
    {
        IContactValidator GetValidator();
        IList<Contact> GetAll();
        Contact GetObjectById(int Id);
        Contact GetObjectByName(string Name);
        Contact CreateObject(Contact contact);
        Contact UpdateObject(Contact contact);
        Contact SoftDeleteObject(Contact contact, IPurchaseOrderService _purchaseOrderService, ISalesOrderService _salesOrderService);
        bool DeleteObject(int Id);
    }
}
