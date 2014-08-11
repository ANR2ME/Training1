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
    public class SalesOrderDetailValidator : ISalesOrderDetailValidator
    {
        public SalesOrderDetail VHasSalesOrder(SalesOrderDetail salesOrderDetail, ISalesOrderService _salesOrderService)
        {
            SalesOrder so = _salesOrderService.GetObjectById(salesOrderDetail.SalesOrderId);
            if (so == null)
            {
                salesOrderDetail.Errors.Add("SalesOrder", "Harus Ada");
            }
            return salesOrderDetail;
        }

        public SalesOrderDetail VHasItem(SalesOrderDetail salesOrderDetail, IItemService _itemService)
        {
            Item i = _itemService.GetObjectById(salesOrderDetail.ItemId);
            if (i == null)
            {
                salesOrderDetail.Errors.Add("Item", "Harus Ada");
            }
            return salesOrderDetail;
        }

        public SalesOrderDetail VIsNotConfirmed(SalesOrderDetail salesOrderDetail)
        {
            if (salesOrderDetail.IsConfirmed)
            {
                salesOrderDetail.Errors.Add("IsConfirmed", "Harus False");
            }
            return salesOrderDetail;
        }

        public SalesOrderDetail VIsPositiveQuantity(SalesOrderDetail salesOrderDetail)
        {
            if (salesOrderDetail.Quantity <= 0)
            {
                salesOrderDetail.Errors.Add("Quantity", "Harus lebih besar dari 0");
            }
            return salesOrderDetail;
        }

        public SalesOrderDetail VIsQuantityValid(SalesOrderDetail salesOrderDetail, IItemService _itemService)
        {
            Item item = _itemService.GetObjectById(salesOrderDetail.ItemId);
            if (item == null || item.Quantity < salesOrderDetail.Quantity)
            {
                salesOrderDetail.Errors.Add("Item Ready", "Harus lebih besar atau sama dengan Quantity");
            }
            return salesOrderDetail;
        }

        public SalesOrderDetail VIsUnconfirmQuantityValid(SalesOrderDetail salesOrderDetail, IItemService _itemService)
        {
            Item item = _itemService.GetObjectById(salesOrderDetail.ItemId);
            if (item == null || item.PendingDelivery < salesOrderDetail.Quantity)
            {
                salesOrderDetail.Errors.Add("Item PendingDelivery", "Harus lebih besar atau sama dengan Quantity");
            }
            return salesOrderDetail;
        }

        public SalesOrderDetail VIsItemUnique(SalesOrderDetail salesOrderDetail, ISalesOrderDetailService _salesOrderDetailService)
        {
            IList<SalesOrderDetail> salesOrderDetails = _salesOrderDetailService.GetObjectsByItemId(salesOrderDetail.ItemId);
            int same = 0;
            foreach (var d in salesOrderDetails)
            {
                if (d.ItemId == salesOrderDetail.ItemId && d.SalesOrderId == salesOrderDetail.SalesOrderId && !d.IsDeleted) same++;
            }
            if (same > 0)
            {
                salesOrderDetail.Errors.Add("Item", "Harus unik");
            }
            return salesOrderDetail;
        }

        public SalesOrderDetail VDontHaveDeliveryOrderDetails(SalesOrderDetail salesOrderDetail, IDeliveryOrderDetailService _deliveryOrderDetailService)
        {
            IList<DeliveryOrderDetail> deliveryOrderDetails = _deliveryOrderDetailService.GetObjectsBySalesOrderDetailId(salesOrderDetail.Id);
            if (deliveryOrderDetails.Any())
            {
                salesOrderDetail.Errors.Add("DeliveryOrderDetails", "Harus tidak terasosiasi");
            }
            return salesOrderDetail;
        }

        public SalesOrderDetail VCreateObject(SalesOrderDetail salesOrderDetail, ISalesOrderDetailService _salesOrderDetailService, ISalesOrderService _salesOrderService, IItemService _itemService)
        {

            VHasSalesOrder(salesOrderDetail, _salesOrderService);
            VHasItem(salesOrderDetail, _itemService);
            VIsPositiveQuantity(salesOrderDetail);
            VIsItemUnique(salesOrderDetail, _salesOrderDetailService);
            VIsQuantityValid(salesOrderDetail, _itemService);
            return salesOrderDetail;
        }

        public SalesOrderDetail VUpdateObject(SalesOrderDetail salesOrderDetail, ISalesOrderDetailService _salesOrderDetailService, ISalesOrderService _salesOrderService, IItemService _itemService)
        {
            VHasSalesOrder(salesOrderDetail, _salesOrderService);
            VHasItem(salesOrderDetail, _itemService);
            VIsPositiveQuantity(salesOrderDetail);
            VIsItemUnique(salesOrderDetail, _salesOrderDetailService);
            VIsNotConfirmed(salesOrderDetail);
            VIsQuantityValid(salesOrderDetail, _itemService);
            return salesOrderDetail;
        }

        public SalesOrderDetail VDeleteObject(SalesOrderDetail salesOrderDetail, ISalesOrderDetailService _salesOrderDetailService)
        {
            VIsNotConfirmed(salesOrderDetail);
            return salesOrderDetail;
        }

        public SalesOrderDetail VConfirmObject(SalesOrderDetail salesOrderDetail, IItemService _itemService)
        {
            VIsNotConfirmed(salesOrderDetail);
            VIsQuantityValid(salesOrderDetail, _itemService);
            return salesOrderDetail;
        }

        public SalesOrderDetail VUnconfirmObject(SalesOrderDetail salesOrderDetail, IItemService _itemService, IDeliveryOrderDetailService _deliveryOrderDetailService)
        {
            VIsUnconfirmQuantityValid(salesOrderDetail, _itemService);
            VDontHaveDeliveryOrderDetails(salesOrderDetail, _deliveryOrderDetailService);
            //VIsNotConfirmed(salesOrderDetail);
            return salesOrderDetail;
        }

        public SalesOrderDetail VReceiveObject(SalesOrderDetail salesOrderDetail)
        {

            return salesOrderDetail;
        }

        public bool ValidCreateObject(SalesOrderDetail salesOrderDetail, ISalesOrderDetailService _salesOrderDetailService, ISalesOrderService _salesOrderService, IItemService _itemService)
        {
            VCreateObject(salesOrderDetail, _salesOrderDetailService, _salesOrderService, _itemService);
            return isValid(salesOrderDetail);
        }

        public bool ValidUpdateObject(SalesOrderDetail salesOrderDetail, ISalesOrderDetailService _salesOrderDetailService, ISalesOrderService _salesOrderService, IItemService _itemService)
        {
            salesOrderDetail.Errors.Clear();
            VUpdateObject(salesOrderDetail, _salesOrderDetailService, _salesOrderService, _itemService);
            return isValid(salesOrderDetail);
        }

        public bool ValidDeleteObject(SalesOrderDetail salesOrderDetail, ISalesOrderDetailService _salesOrderDetailService)
        {
            salesOrderDetail.Errors.Clear();
            VDeleteObject(salesOrderDetail, _salesOrderDetailService);
            return isValid(salesOrderDetail);
        }

        public bool ValidConfirmObject(SalesOrderDetail salesOrderDetail, IItemService _itemService)
        {
            salesOrderDetail.Errors.Clear();
            VConfirmObject(salesOrderDetail, _itemService);
            return isValid(salesOrderDetail);
        }

        public bool ValidUnconfirmObject(SalesOrderDetail salesOrderDetail, IItemService _itemService, IDeliveryOrderDetailService _deliveryOrderDetailService)
        {
            salesOrderDetail.Errors.Clear();
            VUnconfirmObject(salesOrderDetail, _itemService, _deliveryOrderDetailService);
            return isValid(salesOrderDetail);
        }

        public bool ValidReceiveObject(SalesOrderDetail salesOrderDetail)
        {
            salesOrderDetail.Errors.Clear();
            VReceiveObject(salesOrderDetail);
            return isValid(salesOrderDetail);
        }

        public bool isValid(SalesOrderDetail obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }

        public string PrintError(SalesOrderDetail obj)
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
