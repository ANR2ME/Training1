using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DomainModel;
using Core.Interface.Service;
using Core.Interface.Validation;

namespace Validation.Validation
{
    public class ContactValidator : IContactValidator
    {
        public Contact VHasName(Contact contact)
        {
            if (contact.Name == "")
            {
                contact.Errors.Add("Name", "Tidak boleh kosong");
            }
            return contact;
        }

        public Contact VHasAddress(Contact contact)
        {
            if (contact.Address == "")
            {
                contact.Errors.Add("Address", "Tidak boleh kosong");
            }
            return contact;
        }

        public Contact VHasPhoneNumber(Contact contact)
        {
            if (contact.PhoneNumber == "")
            {
                contact.Errors.Add("PhoneNumber", "Tidak boleh kosong");
            }
            return contact;
        }

        public Contact VHasPurchaseOrders(Contact contact, IPurchaseOrderService _purchaseOrderService)
        {
            IList<PurchaseOrder> purchaseOrders = _purchaseOrderService.GetObjectsByContactId(contact.Id);
            if (purchaseOrders.Any())
            {
                contact.Errors.Add("PurchaseOrders", "Tidak boleh terasosiasi");
            }
            return contact;
        }

        public Contact VHasSalesOrders(Contact contact, ISalesOrderService _salesOrderService)
        {
            IList<SalesOrder> salesOrders = _salesOrderService.GetObjectsByContactId(contact.Id);
            if (salesOrders.Any())
            {
                contact.Errors.Add("SalesOrders", "Tidak boleh terasosiasi");
            }
            return contact;
        }

        public Contact VCreateObject(Contact contact)
        {
            VHasName(contact);
            VHasAddress(contact);
            VHasPhoneNumber(contact);
            return contact;
        }

        public Contact VUpdateObject(Contact contact)
        {
            VHasName(contact);
            VHasAddress(contact);
            VHasPhoneNumber(contact);
            return contact;
        }

        public Contact VDeleteObject(Contact contact, IPurchaseOrderService _purchaseOrderService, ISalesOrderService _salesOrderService)
        {
            VHasPurchaseOrders(contact, _purchaseOrderService);
            VHasSalesOrders(contact, _salesOrderService);
            return contact;
        }

        public bool ValidCreateObject(Contact contact)
        {
            VCreateObject(contact);
            return isValid(contact);
        }

        public bool ValidUpdateObject(Contact contact)
        {
            contact.Errors.Clear();
            VUpdateObject(contact);
            return isValid(contact);
        }

        public bool ValidDeleteObject(Contact contact, IPurchaseOrderService _purchaseOrderService, ISalesOrderService _salesOrderService)
        {
            contact.Errors.Clear();
            VDeleteObject(contact, _purchaseOrderService, _salesOrderService);
            return isValid(contact);
        }

        public bool isValid(Contact obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }

        public string PrintError(Contact obj)
        {
            string erroroutput = "";
            KeyValuePair<string, string> first = obj.Errors.ElementAt(0);
            erroroutput += first.Key + "," + first.Value;
            foreach (KeyValuePair<string, string> pair in obj.Errors.Skip(1))
            {
                erroroutput += Environment.NewLine;
                erroroutput += pair.Key + "," + pair.Value;
            }
            return erroroutput;
        }

    }
}
