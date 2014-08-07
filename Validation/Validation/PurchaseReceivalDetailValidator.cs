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
    public class PurchaseReceivalDetailValidator : IPurchaseReceivalDetailValidator
    {
        public PurchaseReceivalDetail VHasPurchaseReceival(PurchaseReceivalDetail purchaseReceivalDetail)
        {
            if (purchaseReceivalDetail.PurchaseReceivalId <= 0)
            {
                purchaseReceivalDetail.Errors.Add("PurchaseReceival", "Harus Ada");
            }
            return purchaseReceivalDetail;
        }

        public PurchaseReceivalDetail VHasItem(PurchaseReceivalDetail purchaseReceivalDetail)
        {
            if (purchaseReceivalDetail.ItemId <= 0)
            {
                purchaseReceivalDetail.Errors.Add("Item", "Harus Ada");
            }
            return purchaseReceivalDetail;
        }

        public PurchaseReceivalDetail VHasPurchaseOrderDetail(PurchaseReceivalDetail purchaseReceivalDetail)
        {
            if (purchaseReceivalDetail.PurchaseOrderDetailId <= 0)
            {
                purchaseReceivalDetail.Errors.Add("PurchaseOrderDetail", "Harus Ada");
            }
            return purchaseReceivalDetail;
        }

        public PurchaseReceivalDetail VIsNotConfirmed(PurchaseReceivalDetail purchaseReceivalDetail)
        {
            if (purchaseReceivalDetail.IsConfirmed)
            {
                purchaseReceivalDetail.Errors.Add("IsConfirmed", "Harus False");
            }
            return purchaseReceivalDetail;
        }

        public PurchaseReceivalDetail VIsConfirmed(PurchaseReceivalDetail purchaseReceivalDetail)
        {
            if (!purchaseReceivalDetail.IsConfirmed)
            {
                purchaseReceivalDetail.Errors.Add("IsConfirmed", "Harus True");
            }
            return purchaseReceivalDetail;
        }

        public PurchaseReceivalDetail VIsPositiveQuantity(PurchaseReceivalDetail purchaseReceivalDetail)
        {
            if (purchaseReceivalDetail.Quantity <= 0)
            {
                purchaseReceivalDetail.Errors.Add("Quantity", "Harus lebih besar dari 0");
            }
            return purchaseReceivalDetail;
        }

        /*public PurchaseReceivalDetail VIsConfirmQuantityValid(PurchaseReceivalDetail purchaseReceivalDetail, IItemService _itemService)
        {
            Item item = _itemService.GetObjectById(purchaseReceivalDetail.ItemId);
            if (purchaseReceivalDetail.Quantity + item.Quantity < 0)
            {
                purchaseReceivalDetail.Errors.Add("Quantity + Item Quantity", "Harus lebih besar atau sama dengan 0");
            }
            return purchaseReceivalDetail;
        }*/

        public PurchaseReceivalDetail VIsUnconfirmQuantityValid(PurchaseReceivalDetail purchaseReceivalDetail, IItemService _itemService)
        {
            Item item = _itemService.GetObjectById(purchaseReceivalDetail.ItemId);
            if (item.Quantity < purchaseReceivalDetail.Quantity)
            {
                purchaseReceivalDetail.Errors.Add("Item Ready", "Harus lebih besar atau sama dengan Quantity");
            }
            return purchaseReceivalDetail;
        }

        public PurchaseReceivalDetail VIsItemUnique(PurchaseReceivalDetail purchaseReceivalDetail, IPurchaseReceivalDetailService _purchaseReceivalDetailService)
        {
            IList<PurchaseReceivalDetail> purchaseReceivalDetails = _purchaseReceivalDetailService.GetObjectsByPurchaseReceivalId(purchaseReceivalDetail.PurchaseReceivalId);
            bool unique = true;
            foreach (var sad in purchaseReceivalDetails)
            {
                if (sad.ItemId == purchaseReceivalDetail.ItemId && sad.Id != purchaseReceivalDetail.Id && !sad.IsDeleted)
                {
                    unique = false;
                    break;
                }
            }
            if (!unique)
            {
                purchaseReceivalDetail.Errors.Add("Item", "Harus unik");
            }
            return purchaseReceivalDetail;
        }

        public PurchaseReceivalDetail VIsValidOrderQuantity(PurchaseReceivalDetail purchaseReceivalDetail, IPurchaseOrderDetailService _purchaseOrderDetailService)
        {
            PurchaseOrderDetail x = _purchaseOrderDetailService.GetObjectById(purchaseReceivalDetail.PurchaseOrderDetailId);
            if (purchaseReceivalDetail.Quantity > x.Quantity)
            {
                purchaseReceivalDetail.Errors.Add("Quantity", "Harus lebih kecil atau sama dengan PurchaseOrderDetail Quantity");
            }
            return purchaseReceivalDetail;
        }

        public PurchaseReceivalDetail VIsOrderDetailConfirmed(PurchaseReceivalDetail purchaseReceivalDetail, IPurchaseOrderDetailService _purchaseOrderDetailService)
        {
            PurchaseOrderDetail x = _purchaseOrderDetailService.GetObjectById(purchaseReceivalDetail.PurchaseOrderDetailId);
            if (!x.IsConfirmed)
            {
                purchaseReceivalDetail.Errors.Add("PurchaseOrderDetail", "Harus Terkonfirmasi");
            }
            return purchaseReceivalDetail;
        }

        public PurchaseReceivalDetail VCreateObject(PurchaseReceivalDetail purchaseReceivalDetail, IPurchaseReceivalDetailService _purchaseReceivalDetailService, IPurchaseOrderDetailService _purchaseOrderDetailService)
        {
            VHasPurchaseReceival(purchaseReceivalDetail);
            VHasItem(purchaseReceivalDetail);
            VHasPurchaseOrderDetail(purchaseReceivalDetail);
            VIsPositiveQuantity(purchaseReceivalDetail);
            VIsItemUnique(purchaseReceivalDetail, _purchaseReceivalDetailService);
            VIsValidOrderQuantity(purchaseReceivalDetail, _purchaseOrderDetailService);
            return purchaseReceivalDetail;
        }

        public PurchaseReceivalDetail VUpdateObject(PurchaseReceivalDetail purchaseReceivalDetail, IPurchaseReceivalDetailService _purchaseReceivalDetailService)
        {
            VHasPurchaseReceival(purchaseReceivalDetail);
            VHasItem(purchaseReceivalDetail);
            VHasPurchaseOrderDetail(purchaseReceivalDetail);
            VIsPositiveQuantity(purchaseReceivalDetail);
            VIsItemUnique(purchaseReceivalDetail, _purchaseReceivalDetailService);
            VIsNotConfirmed(purchaseReceivalDetail);
            return purchaseReceivalDetail;
        }

        public PurchaseReceivalDetail VDeleteObject(PurchaseReceivalDetail purchaseReceivalDetail, IPurchaseReceivalDetailService _purchaseReceivalDetailService)
        {
            VIsNotConfirmed(purchaseReceivalDetail);
            return purchaseReceivalDetail;
        }

        public PurchaseReceivalDetail VConfirmObject(PurchaseReceivalDetail purchaseReceivalDetail, IItemService _itemService, IPurchaseOrderDetailService _purchaseOrderDetailService)
        {
            VIsNotConfirmed(purchaseReceivalDetail);
            VIsOrderDetailConfirmed(purchaseReceivalDetail, _purchaseOrderDetailService);
            return purchaseReceivalDetail;
        }

        public PurchaseReceivalDetail VUnconfirmObject(PurchaseReceivalDetail purchaseReceivalDetail, IItemService _itemService)
        {
            VIsConfirmed(purchaseReceivalDetail);
            VIsUnconfirmQuantityValid(purchaseReceivalDetail, _itemService);
            return purchaseReceivalDetail;
        }

        public PurchaseReceivalDetail VReceiveObject(PurchaseReceivalDetail purchaseReceivalDetail)
        {

            return purchaseReceivalDetail;
        }

        public bool ValidCreateObject(PurchaseReceivalDetail purchaseReceivalDetail, IPurchaseReceivalDetailService _purchaseReceivalDetailService, IPurchaseOrderDetailService _purchaseOrderDetailService)
        {
            VCreateObject(purchaseReceivalDetail, _purchaseReceivalDetailService, _purchaseOrderDetailService);
            return isValid(purchaseReceivalDetail);
        }

        public bool ValidUpdateObject(PurchaseReceivalDetail purchaseReceivalDetail, IPurchaseReceivalDetailService _purchaseReceivalDetailService)
        {
            purchaseReceivalDetail.Errors.Clear();
            VUpdateObject(purchaseReceivalDetail, _purchaseReceivalDetailService);
            return isValid(purchaseReceivalDetail);
        }

        public bool ValidDeleteObject(PurchaseReceivalDetail purchaseReceivalDetail, IPurchaseReceivalDetailService _purchaseReceivalDetailService)
        {
            purchaseReceivalDetail.Errors.Clear();
            VDeleteObject(purchaseReceivalDetail, _purchaseReceivalDetailService);
            return isValid(purchaseReceivalDetail);
        }

        public bool ValidConfirmObject(PurchaseReceivalDetail purchaseReceivalDetail, IItemService _itemService, IPurchaseOrderDetailService _purchaseOrderDetailService)
        {
            purchaseReceivalDetail.Errors.Clear();
            VConfirmObject(purchaseReceivalDetail, _itemService, _purchaseOrderDetailService);
            return isValid(purchaseReceivalDetail);
        }

        public bool ValidUnconfirmObject(PurchaseReceivalDetail purchaseReceivalDetail, IItemService _itemService)
        {
            purchaseReceivalDetail.Errors.Clear();
            VUnconfirmObject(purchaseReceivalDetail, _itemService);
            return isValid(purchaseReceivalDetail);
        }

        public bool ValidReceiveObject(PurchaseReceivalDetail purchaseReceivalDetail)
        {
            purchaseReceivalDetail.Errors.Clear();
            VReceiveObject(purchaseReceivalDetail);
            return isValid(purchaseReceivalDetail);
        }

        public bool isValid(PurchaseReceivalDetail obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }

        public string PrintError(PurchaseReceivalDetail obj)
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
