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
    public class DeliveryOrderValidator : IDeliveryOrderValidator
    {
        public DeliveryOrder VHasSalesOrder(DeliveryOrder deliveryOrder, ISalesOrderService _salesOrderService)
        {
            SalesOrder so = _salesOrderService.GetObjectById(deliveryOrder.SalesOrderId);
            if (so == null)
            {
                deliveryOrder.Errors.Add("SalesOrder", "Harus ada");
            }
            return deliveryOrder;
        }

        public DeliveryOrder VHasContact(DeliveryOrder deliveryOrder, ISalesOrderService _salesOrderService, IContactService _contactService)
        {
            SalesOrder so = _salesOrderService.GetObjectById(deliveryOrder.SalesOrderId);
            Contact c = _contactService.GetObjectById(so.ContactId);
            if (c == null)
            {
                deliveryOrder.Errors.Add("Contact", "Harus ada");
            }
            return deliveryOrder;
        }

        public DeliveryOrder VIsValidDeliveryDate(DeliveryOrder deliveryOrder)
        {
            if (deliveryOrder.DeliveryDate == null)
            {
                deliveryOrder.Errors.Add("DeliveryDate", "Tidak Valid");
            }
            return deliveryOrder;
        }

        public DeliveryOrder VIsConfirmed(DeliveryOrder deliveryOrder)
        {
            if (!deliveryOrder.IsConfirmed)
            {
                deliveryOrder.Errors.Add("IsConfirmed", "Harus True");
            }
            return deliveryOrder;
        }

        public DeliveryOrder VIsNotConfirmed(DeliveryOrder deliveryOrder)
        {
            if (deliveryOrder.IsConfirmed)
            {
                deliveryOrder.Errors.Add("IsConfirmed", "Harus False");
            }
            return deliveryOrder;
        }

        public DeliveryOrder VIsDeliveryOrderDetailsNotConfirmed(DeliveryOrder deliveryOrder, IDeliveryOrderDetailService _deliveryOrderDetailService)
        {
            IList<DeliveryOrderDetail> deliveryOrderDetails = _deliveryOrderDetailService.GetConfirmedObjectsByDeliveryOrderId(deliveryOrder.Id);
            if (deliveryOrderDetails.Any())
            {
                deliveryOrder.Errors.Add("DeliveryOrderDetails", "Harus tidak terkonfirmasi semua");
            }
            return deliveryOrder;
        }

        public DeliveryOrder VHasDeliveryOrderDetails(DeliveryOrder deliveryOrder, IDeliveryOrderDetailService _deliveryOrderDetailService)
        {
            IList<DeliveryOrderDetail> deliveryOrderDetails = _deliveryOrderDetailService.GetObjectsByDeliveryOrderId(deliveryOrder.Id);
            if (!deliveryOrderDetails.Any())
            {
                deliveryOrder.Errors.Add("DeliveryOrderDetails", "Harus ada");
            }
            return deliveryOrder;
        }

        public DeliveryOrder VIsValidDeliveryOrderDetailsQuantity(DeliveryOrder deliveryOrder, IDeliveryOrderDetailService _deliveryOrderDetailService, IItemService _itemService)
        {
            IList<DeliveryOrderDetail> deliveryOrderDetails = _deliveryOrderDetailService.GetObjectsByDeliveryOrderId(deliveryOrder.Id);
            bool valid = true;
            foreach (var sad in deliveryOrderDetails)
            {
                Item item = _itemService.GetObjectById(sad.ItemId);
                if (item.Quantity - sad.Quantity < 0)
                {
                    valid = false;
                    break;
                }
            }
            if (!valid)
            {
                deliveryOrder.Errors.Add("Item Quantity - DeliveryOrderDetails Quantity", "Harus lebih besar atau sama dengan 0");
            }
            return deliveryOrder;
        }

        public DeliveryOrder VCreateObject(DeliveryOrder deliveryOrder, ISalesOrderService _salesOrderService, IContactService _contactService)
        {
            VHasSalesOrder(deliveryOrder, _salesOrderService);
            VHasContact(deliveryOrder, _salesOrderService, _contactService);
            VIsValidDeliveryDate(deliveryOrder);
            return deliveryOrder;
        }

        public DeliveryOrder VUpdateObject(DeliveryOrder deliveryOrder, ISalesOrderService _salesOrderService, IContactService _contactService)
        {
            VHasSalesOrder(deliveryOrder, _salesOrderService);
            VHasContact(deliveryOrder, _salesOrderService, _contactService);
            VIsValidDeliveryDate(deliveryOrder);
            VIsNotConfirmed(deliveryOrder);
            return deliveryOrder;
        }

        public DeliveryOrder VDeleteObject(DeliveryOrder deliveryOrder, IDeliveryOrderDetailService _deliveryOrderDetailService)
        {
            VIsNotConfirmed(deliveryOrder);
            VIsDeliveryOrderDetailsNotConfirmed(deliveryOrder, _deliveryOrderDetailService);
            return deliveryOrder;
        }

        public DeliveryOrder VConfirmObject(DeliveryOrder deliveryOrder, IDeliveryOrderDetailService _deliveryOrderDetailService, IItemService _itemService)
        {
            VHasDeliveryOrderDetails(deliveryOrder, _deliveryOrderDetailService);
            VIsValidDeliveryOrderDetailsQuantity(deliveryOrder, _deliveryOrderDetailService, _itemService);
            //VIsNotConfirmed(deliveryOrder);
            return deliveryOrder;
        }

        public DeliveryOrder VUnconfirmObject(DeliveryOrder deliveryOrder)
        {
            VIsConfirmed(deliveryOrder);
            //VIsDeliveryOrderDetailsNotConfirmed(deliveryOrder, _deliveryOrderDetailService);
            return deliveryOrder;
        }

        public bool ValidCreateObject(DeliveryOrder deliveryOrder, ISalesOrderService _salesOrderService, IContactService _contactService)
        {
            VCreateObject(deliveryOrder, _salesOrderService, _contactService);
            return isValid(deliveryOrder);
        }

        public bool ValidUpdateObject(DeliveryOrder deliveryOrder, ISalesOrderService _salesOrderService, IContactService _contactService)
        {
            deliveryOrder.Errors.Clear();
            VUpdateObject(deliveryOrder, _salesOrderService, _contactService);
            return isValid(deliveryOrder);
        }

        public bool ValidDeleteObject(DeliveryOrder deliveryOrder, IDeliveryOrderDetailService _deliveryOrderDetailService)
        {
            deliveryOrder.Errors.Clear();
            VDeleteObject(deliveryOrder, _deliveryOrderDetailService);
            return isValid(deliveryOrder);
        }

        public bool ValidConfirmObject(DeliveryOrder deliveryOrder, IDeliveryOrderDetailService _deliveryOrderDetailService, IItemService _itemService)
        {
            deliveryOrder.Errors.Clear();
            VConfirmObject(deliveryOrder, _deliveryOrderDetailService, _itemService);
            return isValid(deliveryOrder);
        }

        public bool ValidUnconfirmObject(DeliveryOrder deliveryOrder)
        {
            deliveryOrder.Errors.Clear();
            VUnconfirmObject(deliveryOrder);
            return isValid(deliveryOrder);
        }

        public bool isValid(DeliveryOrder obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }

        public string PrintError(DeliveryOrder obj)
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
