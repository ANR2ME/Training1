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
    public class SalesOrderValidator : ISalesOrderValidator
    {
        public SalesOrder VHasContact(SalesOrder salesOrder, IContactService _contactService)
        {
            Contact contact = _contactService.GetObjectById(salesOrder.ContactId);
            if (contact == null)
            {
                salesOrder.Errors.Add("Contact", "Harus ada");
            }
            return salesOrder;
        }

        public SalesOrder VIsValidSalesDate(SalesOrder salesOrder)
        {
            if (salesOrder.SalesDate == null || salesOrder.SalesDate.Equals(DateTime.FromBinary(0)))
            {
                salesOrder.Errors.Add("SalesDate", "Tidak Valid");
            }
            return salesOrder;
        }

        public SalesOrder VIsConfirmed(SalesOrder salesOrder)
        {
            if (!salesOrder.IsConfirmed)
            {
                salesOrder.Errors.Add("IsConfirmed", "Harus True");
            }
            return salesOrder;
        }

        public SalesOrder VIsNotConfirmed(SalesOrder salesOrder)
        {
            if (salesOrder.IsConfirmed)
            {
                salesOrder.Errors.Add("IsConfirmed", "Harus False");
            }
            return salesOrder;
        }

        public SalesOrder VIsSalesOrderDetailsNotConfirmed(SalesOrder salesOrder, ISalesOrderDetailService _salesOrderDetailService)
        {
            IList<SalesOrderDetail> salesOrderDetails = _salesOrderDetailService.GetConfirmedObjectsBySalesOrderId(salesOrder.Id);
            if (salesOrderDetails.Any())
            {
                salesOrder.Errors.Add("SalesOrderDetails", "Harus tidak terkonfirmasi semua");
            }
            return salesOrder;
        }

        public SalesOrder VHasSalesOrderDetails(SalesOrder salesOrder, ISalesOrderDetailService _salesOrderDetailService)
        {
            IList<SalesOrderDetail> salesOrderDetails = _salesOrderDetailService.GetObjectsBySalesOrderId(salesOrder.Id);
            if (!salesOrderDetails.Any())
            {
                salesOrder.Errors.Add("SalesOrderDetails", "Harus ada");
            }
            return salesOrder;
        }

        public SalesOrder VIsValidSalesOrderDetailsQuantity(SalesOrder salesOrder, ISalesOrderDetailService _salesOrderDetailService, IItemService _itemService)
        {
            IList<SalesOrderDetail> salesOrderDetails = _salesOrderDetailService.GetObjectsBySalesOrderId(salesOrder.Id);
            bool valid = true;
            foreach (var sad in salesOrderDetails)
            {
                Item item = _itemService.GetObjectById(sad.ItemId);
                if (item.Quantity - sad.Quantity < 0) // should be item.Quantity - total salesOrderDetails.Quantity ?
                {
                    valid = false;
                    break;
                }
            }
            if (!valid)
            {
                salesOrder.Errors.Add("Item Quantity - SalesOrderDetails Quantity", "Harus lebih besar atau sama dengan 0");
            }
            return salesOrder;
        }

        public SalesOrder VCreateObject(SalesOrder salesOrder, IContactService _contactService)
        {
            VHasContact(salesOrder, _contactService);
            VIsValidSalesDate(salesOrder);
            return salesOrder;
        }

        public SalesOrder VUpdateObject(SalesOrder salesOrder, IContactService _contactService)
        {
            VHasContact(salesOrder, _contactService);
            VIsValidSalesDate(salesOrder);
            VIsNotConfirmed(salesOrder);
            return salesOrder;
        }

        public SalesOrder VDeleteObject(SalesOrder salesOrder, ISalesOrderDetailService _salesOrderDetailService)
        {
            VIsNotConfirmed(salesOrder);
            VIsSalesOrderDetailsNotConfirmed(salesOrder, _salesOrderDetailService);
            return salesOrder;
        }

        public SalesOrder VConfirmObject(SalesOrder salesOrder, ISalesOrderDetailService _salesOrderDetailService, IItemService _itemService)
        {
            VHasSalesOrderDetails(salesOrder, _salesOrderDetailService);
            VIsValidSalesOrderDetailsQuantity(salesOrder, _salesOrderDetailService, _itemService);
            //VIsNotConfirmed(salesOrder);
            return salesOrder;
        }

        public SalesOrder VUnconfirmObject(SalesOrder salesOrder)
        {
            VIsConfirmed(salesOrder);
            //VIsSalesOrderDetailsNotConfirmed(salesOrder, _salesOrderDetailService);
            return salesOrder;
        }

        public bool ValidCreateObject(SalesOrder salesOrder, IContactService _contactService)
        {
            VCreateObject(salesOrder, _contactService);
            return isValid(salesOrder);
        }

        public bool ValidUpdateObject(SalesOrder salesOrder, IContactService _contactService)
        {
            salesOrder.Errors.Clear();
            VUpdateObject(salesOrder, _contactService);
            return isValid(salesOrder);
        }

        public bool ValidDeleteObject(SalesOrder salesOrder, ISalesOrderDetailService _salesOrderDetailService)
        {
            salesOrder.Errors.Clear();
            VDeleteObject(salesOrder, _salesOrderDetailService);
            return isValid(salesOrder);
        }

        public bool ValidConfirmObject(SalesOrder salesOrder, ISalesOrderDetailService _salesOrderDetailService, IItemService _itemService)
        {
            salesOrder.Errors.Clear();
            VConfirmObject(salesOrder, _salesOrderDetailService, _itemService);
            return isValid(salesOrder);
        }

        public bool ValidUnconfirmObject(SalesOrder salesOrder)
        {
            salesOrder.Errors.Clear();
            VUnconfirmObject(salesOrder);
            return isValid(salesOrder);
        }

        public bool isValid(SalesOrder obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }

        public string PrintError(SalesOrder obj)
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
