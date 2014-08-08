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
    public class PurchaseOrderDetailValidator : IPurchaseOrderDetailValidator
    {
        public PurchaseOrderDetail VHasPurchaseOrder(PurchaseOrderDetail purchaseOrderDetail)
        {
            if (purchaseOrderDetail.PurchaseOrderId <= 0)
            {
                purchaseOrderDetail.Errors.Add("PurchaseOrder", "Harus Ada");
            }
            return purchaseOrderDetail;
        }

        public PurchaseOrderDetail VHasItem(PurchaseOrderDetail purchaseOrderDetail)
        {
            if (purchaseOrderDetail.ItemId <= 0)
            {
                purchaseOrderDetail.Errors.Add("Item", "Harus Ada");
            }
            return purchaseOrderDetail;
        }

        public PurchaseOrderDetail VIsNotConfirmed(PurchaseOrderDetail purchaseOrderDetail)
        {
            if (purchaseOrderDetail.IsConfirmed)
            {
                purchaseOrderDetail.Errors.Add("IsConfirmed", "Harus False");
            }
            return purchaseOrderDetail;
        }

        public PurchaseOrderDetail VIsPositiveQuantity(PurchaseOrderDetail purchaseOrderDetail)
        {
            if (purchaseOrderDetail.Quantity <= 0)
            {
                purchaseOrderDetail.Errors.Add("Quantity", "Harus lebih besar dari 0");
            }
            return purchaseOrderDetail;
        }

        public PurchaseOrderDetail VIsUnconfirmQuantityValid(PurchaseOrderDetail purchaseOrderDetail, IItemService _itemService)
        {
            Item item = _itemService.GetObjectById(purchaseOrderDetail.ItemId);
            if (item.PendingReceival < purchaseOrderDetail.Quantity)
            {
                purchaseOrderDetail.Errors.Add("Item PendingReceival", "Harus lebih besar atau sama dengan Quantity");
            }
            return purchaseOrderDetail;
        }

        public PurchaseOrderDetail VIsItemUnique(PurchaseOrderDetail purchaseOrderDetail, IPurchaseOrderDetailService _purchaseOrderDetailService)
        {
            IList<PurchaseOrderDetail> purchaseOrderDetails = _purchaseOrderDetailService.GetObjectsByPurchaseOrderId(purchaseOrderDetail.PurchaseOrderId);
            bool unique = true;
            foreach (var sad in purchaseOrderDetails)
            {
                if (sad.ItemId == purchaseOrderDetail.ItemId && sad.Id != purchaseOrderDetail.Id && !sad.IsDeleted)
                {
                    unique = false;
                    break;
                }
            }
            if (!unique)
            {
                purchaseOrderDetail.Errors.Add("Item", "Harus unik");
            }
            return purchaseOrderDetail;
        }

        public PurchaseOrderDetail VDontHavePurchaseReceivalDetails(PurchaseOrderDetail purchaseOrderDetail, IPurchaseReceivalDetailService _purchaseReceivalDetailService)
        {
            IList<PurchaseReceivalDetail> purchaseReceivalDetails = _purchaseReceivalDetailService.GetObjectsByPurchaseOrderDetailId(purchaseOrderDetail.Id);
            if (purchaseReceivalDetails.Any())
            {
                purchaseOrderDetail.Errors.Add("PurchaseReceivalDetails", "Harus tidak terasosiasi");
            }
            return purchaseOrderDetail;
        }

        public PurchaseOrderDetail VCreateObject(PurchaseOrderDetail purchaseOrderDetail, IPurchaseOrderDetailService _purchaseOrderDetailService)
        {

            VHasPurchaseOrder(purchaseOrderDetail);
            VHasItem(purchaseOrderDetail);
            VIsPositiveQuantity(purchaseOrderDetail);
            VIsItemUnique(purchaseOrderDetail, _purchaseOrderDetailService);
            return purchaseOrderDetail;
        }

        public PurchaseOrderDetail VUpdateObject(PurchaseOrderDetail purchaseOrderDetail, IPurchaseOrderDetailService _purchaseOrderDetailService)
        {
            VHasPurchaseOrder(purchaseOrderDetail);
            VHasItem(purchaseOrderDetail);
            VIsPositiveQuantity(purchaseOrderDetail);
            VIsItemUnique(purchaseOrderDetail, _purchaseOrderDetailService);
            VIsNotConfirmed(purchaseOrderDetail);
            return purchaseOrderDetail;
        }

        public PurchaseOrderDetail VDeleteObject(PurchaseOrderDetail purchaseOrderDetail, IPurchaseOrderDetailService _purchaseOrderDetailService)
        {
            VIsNotConfirmed(purchaseOrderDetail);
            return purchaseOrderDetail;
        }

        public PurchaseOrderDetail VConfirmObject(PurchaseOrderDetail purchaseOrderDetail, IItemService _itemService)
        {
            VIsNotConfirmed(purchaseOrderDetail);
            return purchaseOrderDetail;
        }

        public PurchaseOrderDetail VUnconfirmObject(PurchaseOrderDetail purchaseOrderDetail, IItemService _itemService, IPurchaseReceivalDetailService _purchaseReceivalDetailService)
        {
            VIsUnconfirmQuantityValid(purchaseOrderDetail, _itemService);
            VDontHavePurchaseReceivalDetails(purchaseOrderDetail, _purchaseReceivalDetailService);
            //VIsConfirmed(purchaseOrderDetail);
            return purchaseOrderDetail;
        }

        public PurchaseOrderDetail VReceiveObject(PurchaseOrderDetail purchaseOrderDetail)
        {
            
            return purchaseOrderDetail;
        }

        public bool ValidCreateObject(PurchaseOrderDetail purchaseOrderDetail, IPurchaseOrderDetailService _purchaseOrderDetailService)
        {
            VCreateObject(purchaseOrderDetail, _purchaseOrderDetailService);
            return isValid(purchaseOrderDetail);
        }

        public bool ValidUpdateObject(PurchaseOrderDetail purchaseOrderDetail, IPurchaseOrderDetailService _purchaseOrderDetailService)
        {
            purchaseOrderDetail.Errors.Clear();
            VUpdateObject(purchaseOrderDetail, _purchaseOrderDetailService);
            return isValid(purchaseOrderDetail);
        }

        public bool ValidDeleteObject(PurchaseOrderDetail purchaseOrderDetail, IPurchaseOrderDetailService _purchaseOrderDetailService)
        {
            purchaseOrderDetail.Errors.Clear();
            VDeleteObject(purchaseOrderDetail, _purchaseOrderDetailService);
            return isValid(purchaseOrderDetail);
        }

        public bool ValidConfirmObject(PurchaseOrderDetail purchaseOrderDetail, IItemService _itemService)
        {
            purchaseOrderDetail.Errors.Clear();
            VConfirmObject(purchaseOrderDetail, _itemService);
            return isValid(purchaseOrderDetail);
        }

        public bool ValidUnconfirmObject(PurchaseOrderDetail purchaseOrderDetail, IItemService _itemService, IPurchaseReceivalDetailService _purchaseReceivalDetailService)
        {
            purchaseOrderDetail.Errors.Clear();
            VUnconfirmObject(purchaseOrderDetail, _itemService, _purchaseReceivalDetailService);
            return isValid(purchaseOrderDetail);
        }

        public bool ValidReceiveObject(PurchaseOrderDetail purchaseOrderDetail)
        {
            purchaseOrderDetail.Errors.Clear();
            VReceiveObject(purchaseOrderDetail);
            return isValid(purchaseOrderDetail);
        }

        public bool isValid(PurchaseOrderDetail obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }

        public string PrintError(PurchaseOrderDetail obj)
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
