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
    public class PurchaseReceivalValidator : IPurchaseReceivalValidator
    {
        public PurchaseReceival VHasPurchaseOrder(PurchaseReceival purchaseReceival)
        {
            if (purchaseReceival.PurchaseOrderId <= 0)
            {
                purchaseReceival.Errors.Add("PurchaseOrder", "Harus ada");
            }
            return purchaseReceival;
        }

        public PurchaseReceival VHasContact(PurchaseReceival purchaseReceival, IPurchaseOrderService _purchaseOrderService)
        {
            PurchaseOrder po = _purchaseOrderService.GetObjectById(purchaseReceival.PurchaseOrderId);
            if (po.ContactId <= 0)
            {
                purchaseReceival.Errors.Add("Contact", "Harus ada");
            }
            return purchaseReceival;
        }

        public PurchaseReceival VIsValidReceivalDate(PurchaseReceival purchaseReceival)
        {
            if (purchaseReceival.ReceivalDate == null)
            {
                purchaseReceival.Errors.Add("ReceivalDate", "Tidak Valid");
            }
            return purchaseReceival;
        }

        public PurchaseReceival VIsConfirmed(PurchaseReceival purchaseReceival)
        {
            if (!purchaseReceival.IsConfirmed)
            {
                purchaseReceival.Errors.Add("IsConfirmed", "Harus True");
            }
            return purchaseReceival;
        }

        public PurchaseReceival VIsNotConfirmed(PurchaseReceival purchaseReceival)
        {
            if (purchaseReceival.IsConfirmed)
            {
                purchaseReceival.Errors.Add("IsConfirmed", "Harus False");
            }
            return purchaseReceival;
        }

        public PurchaseReceival VIsPurchaseReceivalDetailsNotConfirmed(PurchaseReceival purchaseReceival, IPurchaseReceivalDetailService _purchaseReceivalDetailService)
        {
            IList<PurchaseReceivalDetail> purchaseReceivalDetails = _purchaseReceivalDetailService.GetConfirmedObjectsByPurchaseReceivalId(purchaseReceival.Id);
            if (purchaseReceivalDetails.Any())
            {
                purchaseReceival.Errors.Add("PurchaseReceivalDetails", "Harus tidak terkonfirmasi semua");
            }
            return purchaseReceival;
        }

        public PurchaseReceival VHasPurchaseReceivalDetails(PurchaseReceival purchaseReceival, IPurchaseReceivalDetailService _purchaseReceivalDetailService)
        {
            IList<PurchaseReceivalDetail> purchaseReceivalDetails = _purchaseReceivalDetailService.GetObjectsByPurchaseReceivalId(purchaseReceival.Id);
            if (!purchaseReceivalDetails.Any())
            {
                purchaseReceival.Errors.Add("PurchaseReceivalDetails", "Harus ada");
            }
            return purchaseReceival;
        }

        /*public PurchaseReceival VIsValidPurchaseReceivalDetailsQuantity(PurchaseReceival purchaseReceival, IPurchaseReceivalDetailService _purchaseReceivalDetailService, IItemService _itemService)
        {
            IList<PurchaseReceivalDetail> purchaseReceivalDetails = _purchaseReceivalDetailService.GetObjectsByPurchaseReceivalId(purchaseReceival.Id);
            bool valid = true;
            foreach (var sad in purchaseReceivalDetails)
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
                purchaseReceival.Errors.Add("PurchaseReceivalDetails Quantity + Item Quantity", "Harus lebih besar atau sama dengan 0");
            }
            return purchaseReceival;
        }*/

        public PurchaseReceival VCreateObject(PurchaseReceival purchaseReceival, IPurchaseOrderService _purchaseOrderService)
        {
            VHasPurchaseOrder(purchaseReceival);
            //VHasContact(purchaseReceival, _purchaseOrderService);
            VIsValidReceivalDate(purchaseReceival);
            return purchaseReceival;
        }

        public PurchaseReceival VUpdateObject(PurchaseReceival purchaseReceival, IPurchaseOrderService _purchaseOrderService)
        {
            VHasContact(purchaseReceival, _purchaseOrderService);
            VIsValidReceivalDate(purchaseReceival);
            VIsNotConfirmed(purchaseReceival);
            return purchaseReceival;
        }

        public PurchaseReceival VDeleteObject(PurchaseReceival purchaseReceival, IPurchaseReceivalDetailService _purchaseReceivalDetailService)
        {
            VIsNotConfirmed(purchaseReceival);
            VIsPurchaseReceivalDetailsNotConfirmed(purchaseReceival, _purchaseReceivalDetailService);
            return purchaseReceival;
        }

        public PurchaseReceival VConfirmObject(PurchaseReceival purchaseReceival, IPurchaseReceivalDetailService _purchaseReceivalDetailService, IItemService _itemService)
        {
            VHasPurchaseReceivalDetails(purchaseReceival, _purchaseReceivalDetailService);
            //VIsNotConfirmed(purchaseReceival);
            return purchaseReceival;
        }

        public PurchaseReceival VUnconfirmObject(PurchaseReceival purchaseReceival)
        {
            VIsConfirmed(purchaseReceival);
            //VIsPurchaseReceivalDetailsNotConfirmed(purchaseReceival, _purchaseReceivalDetailService);
            return purchaseReceival;
        }

        public bool ValidCreateObject(PurchaseReceival purchaseReceival, IPurchaseOrderService _purchaseOrderService)
        {
            VCreateObject(purchaseReceival, _purchaseOrderService);
            return isValid(purchaseReceival);
        }

        public bool ValidUpdateObject(PurchaseReceival purchaseReceival, IPurchaseOrderService _purchaseOrderService)
        {
            purchaseReceival.Errors.Clear();
            VUpdateObject(purchaseReceival, _purchaseOrderService);
            return isValid(purchaseReceival);
        }

        public bool ValidDeleteObject(PurchaseReceival purchaseReceival, IPurchaseReceivalDetailService _purchaseReceivalDetailService)
        {
            purchaseReceival.Errors.Clear();
            VDeleteObject(purchaseReceival, _purchaseReceivalDetailService);
            return isValid(purchaseReceival);
        }

        public bool ValidConfirmObject(PurchaseReceival purchaseReceival, IPurchaseReceivalDetailService _purchaseReceivalDetailService, IItemService _itemService)
        {
            purchaseReceival.Errors.Clear();
            VConfirmObject(purchaseReceival, _purchaseReceivalDetailService, _itemService);
            return isValid(purchaseReceival);
        }

        public bool ValidUnconfirmObject(PurchaseReceival purchaseReceival)
        {
            purchaseReceival.Errors.Clear();
            VUnconfirmObject(purchaseReceival);
            return isValid(purchaseReceival);
        }

        public bool isValid(PurchaseReceival obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }

        public string PrintError(PurchaseReceival obj)
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
