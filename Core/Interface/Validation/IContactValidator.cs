using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DomainModel;
using Core.Interface.Service;

namespace Core.Interface.Validation
{
    public interface IContactValidator
    {
        Contact VHasName(Contact contact);
        Contact VHasAddress(Contact contact);
        Contact VHasPhoneNumber(Contact contact);
        Contact VHasPurchaseOrders(Contact contact, IPurchaseOrderService _purchaseOrderService);
        Contact VHasSalesOrders(Contact contact, ISalesOrderService _salesOrderService);

        Contact VCreateObject(Contact contact);
        Contact VUpdateObject(Contact contact);
        Contact VDeleteObject(Contact contact, IPurchaseOrderService _purchaseOrderService, ISalesOrderService _salesOrderService);

        bool ValidCreateObject(Contact contact);
        bool ValidUpdateObject(Contact contact);
        bool ValidDeleteObject(Contact contact, IPurchaseOrderService _purchaseOrderService, ISalesOrderService _salesOrderService);
        bool isValid(Contact contact);
        string PrintError(Contact contact);
    }
}
