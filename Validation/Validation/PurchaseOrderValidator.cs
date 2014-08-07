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
    public class PurchaseOrderValidator : IPurchaseOrderValidator
    {
        public PurchaseOrder VHasCustomer (PurchaseOrder purchaseOrder)
        {
            if (purchaseOrder.CustomerId <= 0)
            {
                purchaseOrder.Errors.Add("Customer", "Harus ada");
            }
            return purchaseOrder;
        }

        public PurchaseOrder VIsValidPurchaseDate(PurchaseOrder purchaseOrder)
        {
            if (purchaseOrder.PurchaseDate == null)
            {
                purchaseOrder.Errors.Add("PurchaseDate", "Tidak Valid");
            }
            return purchaseOrder;
        }

        public PurchaseOrder VIsConfirmed(PurchaseOrder purchaseOrder)
        {
            if (!purchaseOrder.IsConfirmed)
            {
                purchaseOrder.Errors.Add("IsConfirmed", "Harus True");
            }
            return purchaseOrder;
        }

        public PurchaseOrder VIsNotConfirmed(PurchaseOrder purchaseOrder)
        {
            if (purchaseOrder.IsConfirmed)
            {
                purchaseOrder.Errors.Add("IsConfirmed", "Harus False");
            }
            return purchaseOrder;
        }

        public PurchaseOrder VIsPurchaseOrderDetailsNotConfirmed(PurchaseOrder purchaseOrder, IPurchaseOrderDetailService _purchaseOrderDetailService)
        {
            IList<PurchaseOrderDetail> purchaseOrderDetails = _purchaseOrderDetailService.GetConfirmedObjectsByPurchaseOrderId(purchaseOrder.Id);
            if (purchaseOrderDetails.Any())
            {
                purchaseOrder.Errors.Add("PurchaseOrderDetails", "Harus tidak terkonfirmasi semua");
            }
            return purchaseOrder;
        }

        public PurchaseOrder VHasPurchaseOrderDetails(PurchaseOrder purchaseOrder, IPurchaseOrderDetailService _purchaseOrderDetailService)
        {
            IList<PurchaseOrderDetail> purchaseOrderDetails = _purchaseOrderDetailService.GetObjectsByPurchaseOrderId(purchaseOrder.Id);
            if (!purchaseOrderDetails.Any())
            {
                purchaseOrder.Errors.Add("PurchaseOrderDetails", "Harus ada");
            }
            return purchaseOrder;
        }

        /*public PurchaseOrder VIsValidPurchaseOrderDetailsQuantity(PurchaseOrder purchaseOrder, IPurchaseOrderDetailService _purchaseOrderDetailService, IItemService _itemService)
        {
            IList<PurchaseOrderDetail> purchaseOrderDetails = _purchaseOrderDetailService.GetObjectsByPurchaseOrderId(purchaseOrder.Id);
            bool valid = true;
            foreach (var sad in purchaseOrderDetails)
            {
                Item item = _itemService.GetObjectById(sad.ItemId);
                if (sad.Quantity + item.Quantity < 0)
                {
                    valid = false;
                    break;
                }
            }
            if (!valid)
            {
                purchaseOrder.Errors.Add("PurchaseOrderDetails Quantity + Item Quantity", "Harus lebih besar atau sama dengan 0");
            }
            return purchaseOrder;
        }*/

        public PurchaseOrder VCreateObject(PurchaseOrder purchaseOrder)
        {
            VHasCustomer(purchaseOrder);
            VIsValidPurchaseDate(purchaseOrder);
            return purchaseOrder;
        }

        public PurchaseOrder VUpdateObject(PurchaseOrder purchaseOrder)
        {
            VHasCustomer(purchaseOrder);
            VIsValidPurchaseDate(purchaseOrder);
            VIsNotConfirmed(purchaseOrder);
            return purchaseOrder;
        }

        public PurchaseOrder VDeleteObject(PurchaseOrder purchaseOrder, IPurchaseOrderDetailService _purchaseOrderDetailService)
        {
            VIsNotConfirmed(purchaseOrder);
            VIsPurchaseOrderDetailsNotConfirmed(purchaseOrder, _purchaseOrderDetailService);
            return purchaseOrder;
        }

        public PurchaseOrder VConfirmObject(PurchaseOrder purchaseOrder, IPurchaseOrderDetailService _purchaseOrderDetailService, IItemService _itemService)
        {
            VHasPurchaseOrderDetails(purchaseOrder, _purchaseOrderDetailService);
            //VIsNotConfirmed(purchaseOrder);
            return purchaseOrder;
        }

        public PurchaseOrder VUnconfirmObject(PurchaseOrder purchaseOrder)
        {
            VIsConfirmed(purchaseOrder);
            //VIsPurchaseOrderDetailsNotConfirmed(purchaseOrder, _purchaseOrderDetailService);
            return purchaseOrder;
        }

        public bool ValidCreateObject(PurchaseOrder purchaseOrder)
        {
            VCreateObject(purchaseOrder);
            return isValid(purchaseOrder);
        }

        public bool ValidUpdateObject(PurchaseOrder purchaseOrder)
        {
            purchaseOrder.Errors.Clear();
            VUpdateObject(purchaseOrder);
            return isValid(purchaseOrder);
        }

        public bool ValidDeleteObject(PurchaseOrder purchaseOrder, IPurchaseOrderDetailService _purchaseOrderDetailService)
        {
            purchaseOrder.Errors.Clear();
            VDeleteObject(purchaseOrder, _purchaseOrderDetailService);
            return isValid(purchaseOrder);
        }

        public bool ValidConfirmObject(PurchaseOrder purchaseOrder, IPurchaseOrderDetailService _purchaseOrderDetailService, IItemService _itemService)
        {
            purchaseOrder.Errors.Clear();
            VConfirmObject(purchaseOrder, _purchaseOrderDetailService, _itemService);
            return isValid(purchaseOrder);
        }

        public bool ValidUnconfirmObject(PurchaseOrder purchaseOrder)
        {
            purchaseOrder.Errors.Clear();
            VUnconfirmObject(purchaseOrder);
            return isValid(purchaseOrder);
        }

        public bool isValid(PurchaseOrder obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }

        public string PrintError(PurchaseOrder obj)
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
