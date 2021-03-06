﻿using System;
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
        public PurchaseOrder VHasContact (PurchaseOrder purchaseOrder, IContactService _contactService)
        {
            Contact c = _contactService.GetObjectById(purchaseOrder.ContactId);
            if (c == null)
            {
                purchaseOrder.Errors.Add("Contact", "Harus ada");
            }
            return purchaseOrder;
        }

        public PurchaseOrder VIsValidPurchaseDate(PurchaseOrder purchaseOrder)
        {
            if (purchaseOrder.PurchaseDate == null || purchaseOrder.PurchaseDate.Equals(DateTime.FromBinary(0)))
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

        public PurchaseOrder VCreateObject(PurchaseOrder purchaseOrder, IContactService _contactService)
        {
            VHasContact(purchaseOrder, _contactService);
            VIsValidPurchaseDate(purchaseOrder);
            return purchaseOrder;
        }

        public PurchaseOrder VUpdateObject(PurchaseOrder purchaseOrder, IContactService _contactService)
        {
            VHasContact(purchaseOrder, _contactService);
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
            return purchaseOrder;
        }

        public PurchaseOrder VUnconfirmObject(PurchaseOrder purchaseOrder)
        {
            VIsConfirmed(purchaseOrder);
            //VIsPurchaseOrderDetailsNotConfirmed(purchaseOrder, _purchaseOrderDetailService);
            return purchaseOrder;
        }

        public bool ValidCreateObject(PurchaseOrder purchaseOrder, IContactService _contactService)
        {
            VCreateObject(purchaseOrder, _contactService);
            return isValid(purchaseOrder);
        }

        public bool ValidUpdateObject(PurchaseOrder purchaseOrder, IContactService _contactService)
        {
            purchaseOrder.Errors.Clear();
            VUpdateObject(purchaseOrder, _contactService);
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
