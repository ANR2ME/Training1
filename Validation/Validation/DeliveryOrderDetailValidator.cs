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
    public class DeliveryOrderDetailValidator : IDeliveryOrderDetailValidator
    {
        public DeliveryOrderDetail VHasDeliveryOrder(DeliveryOrderDetail deliveryOrderDetail)
        {
            if (deliveryOrderDetail.DeliveryOrderId <= 0)
            {
                deliveryOrderDetail.Errors.Add("DeliveryOrder", "Harus Ada");
            }
            return deliveryOrderDetail;
        }

        public DeliveryOrderDetail VHasItem(DeliveryOrderDetail deliveryOrderDetail)
        {
            if (deliveryOrderDetail.ItemId <= 0)
            {
                deliveryOrderDetail.Errors.Add("Item", "Harus Ada");
            }
            return deliveryOrderDetail;
        }

        public DeliveryOrderDetail VHasSalesOrderDetail(DeliveryOrderDetail deliveryOrderDetail)
        {
            if (deliveryOrderDetail.SalesOrderDetailId <= 0)
            {
                deliveryOrderDetail.Errors.Add("SalesOrderDetail", "Harus Ada");
            }
            return deliveryOrderDetail;
        }

        public DeliveryOrderDetail VIsNotConfirmed(DeliveryOrderDetail deliveryOrderDetail)
        {
            if (deliveryOrderDetail.IsConfirmed)
            {
                deliveryOrderDetail.Errors.Add("IsConfirmed", "Harus False");
            }
            return deliveryOrderDetail;
        }

        public DeliveryOrderDetail VIsConfirmed(DeliveryOrderDetail deliveryOrderDetail)
        {
            if (!deliveryOrderDetail.IsConfirmed)
            {
                deliveryOrderDetail.Errors.Add("IsConfirmed", "Harus True");
            }
            return deliveryOrderDetail;
        }

        public DeliveryOrderDetail VIsPositiveQuantity(DeliveryOrderDetail deliveryOrderDetail)
        {
            if (deliveryOrderDetail.Quantity <= 0)
            {
                deliveryOrderDetail.Errors.Add("Quantity", "Harus lebih besar dari 0");
            }
            return deliveryOrderDetail;
        }

        public DeliveryOrderDetail VIsQuantityValid(DeliveryOrderDetail deliveryOrderDetail, IItemService _itemService)
        {
            Item item = _itemService.GetObjectById(deliveryOrderDetail.ItemId);
            if (item.Quantity < deliveryOrderDetail.Quantity)
            {
                deliveryOrderDetail.Errors.Add("Item Ready", "Harus lebih besar atau sama dengan Quantity");
            }
            return deliveryOrderDetail;
        }

        public DeliveryOrderDetail VIsUnconfirmQuantityValid(DeliveryOrderDetail deliveryOrderDetail, IItemService _itemService)
        {
            Item item = _itemService.GetObjectById(deliveryOrderDetail.ItemId);
            if (item.PendingDelivery < deliveryOrderDetail.Quantity)
            {
                deliveryOrderDetail.Errors.Add("Item PendingDelivery", "Harus lebih besar atau sama dengan Quantity");
            }
            return deliveryOrderDetail;
        }

        public DeliveryOrderDetail VIsItemUnique(DeliveryOrderDetail deliveryOrderDetail, IDeliveryOrderDetailService _deliveryOrderDetailService)
        {
            IList<DeliveryOrderDetail> deliveryOrderDetails = _deliveryOrderDetailService.GetObjectsByDeliveryOrderId(deliveryOrderDetail.DeliveryOrderId);
            bool unique = true;
            foreach (var sad in deliveryOrderDetails)
            {
                if (sad.ItemId == deliveryOrderDetail.ItemId && sad.Id != deliveryOrderDetail.Id && !sad.IsDeleted)
                {
                    unique = false;
                    break;
                }
            }
            if (!unique)
            {
                deliveryOrderDetail.Errors.Add("Item", "Harus unik");
            }
            return deliveryOrderDetail;
        }

        public DeliveryOrderDetail VIsValidOrderQuantity(DeliveryOrderDetail deliveryOrderDetail, ISalesOrderDetailService _salesOrderDetailService)
        {
            SalesOrderDetail x = _salesOrderDetailService.GetObjectById(deliveryOrderDetail.SalesOrderDetailId);
            if (deliveryOrderDetail.Quantity > x.Quantity)
            {
                deliveryOrderDetail.Errors.Add("Quantity", "Harus lebih kecil atau sama dengan SalesOrderDetail Quantity");
            }
            return deliveryOrderDetail;
        }

        public DeliveryOrderDetail VIsOrderDetailConfirmed(DeliveryOrderDetail deliveryOrderDetail, ISalesOrderDetailService _salesOrderDetailService)
        {
            SalesOrderDetail x = _salesOrderDetailService.GetObjectById(deliveryOrderDetail.SalesOrderDetailId);
            if (!x.IsConfirmed)
            {
                deliveryOrderDetail.Errors.Add("SalesOrderDetail", "Harus Terkonfirmasi");
            }
            return deliveryOrderDetail;
        }

        public DeliveryOrderDetail VCreateObject(DeliveryOrderDetail deliveryOrderDetail, IDeliveryOrderDetailService _deliveryOrderDetailService, ISalesOrderDetailService _salesOrderDetailService)
        {
            VHasDeliveryOrder(deliveryOrderDetail);
            VHasItem(deliveryOrderDetail);
            VHasSalesOrderDetail(deliveryOrderDetail);
            VIsPositiveQuantity(deliveryOrderDetail);
            VIsItemUnique(deliveryOrderDetail, _deliveryOrderDetailService);
            VIsValidOrderQuantity(deliveryOrderDetail, _salesOrderDetailService);
            return deliveryOrderDetail;
        }

        public DeliveryOrderDetail VUpdateObject(DeliveryOrderDetail deliveryOrderDetail, IDeliveryOrderDetailService _deliveryOrderDetailService)
        {
            VHasDeliveryOrder(deliveryOrderDetail);
            VHasItem(deliveryOrderDetail);
            VHasSalesOrderDetail(deliveryOrderDetail);
            VIsPositiveQuantity(deliveryOrderDetail);
            VIsItemUnique(deliveryOrderDetail, _deliveryOrderDetailService);
            VIsNotConfirmed(deliveryOrderDetail);
            return deliveryOrderDetail;
        }

        public DeliveryOrderDetail VDeleteObject(DeliveryOrderDetail deliveryOrderDetail, IDeliveryOrderDetailService _deliveryOrderDetailService)
        {
            VIsNotConfirmed(deliveryOrderDetail);
            return deliveryOrderDetail;
        }

        public DeliveryOrderDetail VConfirmObject(DeliveryOrderDetail deliveryOrderDetail, IItemService _itemService, ISalesOrderDetailService _salesOrderDetailService)
        {
            VIsNotConfirmed(deliveryOrderDetail);
            VIsOrderDetailConfirmed(deliveryOrderDetail, _salesOrderDetailService);
            VIsQuantityValid(deliveryOrderDetail, _itemService);
            return deliveryOrderDetail;
        }

        public DeliveryOrderDetail VUnconfirmObject(DeliveryOrderDetail deliveryOrderDetail, IItemService _itemService)
        {
            VIsConfirmed(deliveryOrderDetail);
            //VIsUnconfirmQuantityValid(deliveryOrderDetail, _itemService);
            return deliveryOrderDetail;
        }

        public DeliveryOrderDetail VReceiveObject(DeliveryOrderDetail deliveryOrderDetail)
        {

            return deliveryOrderDetail;
        }

        public bool ValidCreateObject(DeliveryOrderDetail deliveryOrderDetail, IDeliveryOrderDetailService _deliveryOrderDetailService, ISalesOrderDetailService _salesOrderDetailService)
        {
            VCreateObject(deliveryOrderDetail, _deliveryOrderDetailService, _salesOrderDetailService);
            return isValid(deliveryOrderDetail);
        }

        public bool ValidUpdateObject(DeliveryOrderDetail deliveryOrderDetail, IDeliveryOrderDetailService _deliveryOrderDetailService)
        {
            deliveryOrderDetail.Errors.Clear();
            VUpdateObject(deliveryOrderDetail, _deliveryOrderDetailService);
            return isValid(deliveryOrderDetail);
        }

        public bool ValidDeleteObject(DeliveryOrderDetail deliveryOrderDetail, IDeliveryOrderDetailService _deliveryOrderDetailService)
        {
            deliveryOrderDetail.Errors.Clear();
            VDeleteObject(deliveryOrderDetail, _deliveryOrderDetailService);
            return isValid(deliveryOrderDetail);
        }

        public bool ValidConfirmObject(DeliveryOrderDetail deliveryOrderDetail, IItemService _itemService, ISalesOrderDetailService _salesOrderDetailService)
        {
            deliveryOrderDetail.Errors.Clear();
            VConfirmObject(deliveryOrderDetail, _itemService, _salesOrderDetailService);
            return isValid(deliveryOrderDetail);
        }

        public bool ValidUnconfirmObject(DeliveryOrderDetail deliveryOrderDetail, IItemService _itemService)
        {
            deliveryOrderDetail.Errors.Clear();
            VUnconfirmObject(deliveryOrderDetail, _itemService);
            return isValid(deliveryOrderDetail);
        }

        public bool ValidReceiveObject(DeliveryOrderDetail deliveryOrderDetail)
        {
            deliveryOrderDetail.Errors.Clear();
            VReceiveObject(deliveryOrderDetail);
            return isValid(deliveryOrderDetail);
        }

        public bool isValid(DeliveryOrderDetail obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }

        public string PrintError(DeliveryOrderDetail obj)
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
