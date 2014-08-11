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
    public class StockAdjustmentDetailValidator : IStockAdjustmentDetailValidator
    {
        public StockAdjustmentDetail VHasStockAdjustment(StockAdjustmentDetail stockAdjustmentDetail, IStockAdjustmentService _stockAdjustmentService)
        {
            StockAdjustment sa = _stockAdjustmentService.GetObjectById(stockAdjustmentDetail.StockAdjustmentId);
            if (sa == null)
            {
                stockAdjustmentDetail.Errors.Add("StockAdjustment", "Harus Ada");
            }
            return stockAdjustmentDetail;
        }

        public StockAdjustmentDetail VHasItem(StockAdjustmentDetail stockAdjustmentDetail, IItemService _itemService)
        {
            Item i = _itemService.GetObjectById(stockAdjustmentDetail.ItemId);
            if (i == null)
            {
                stockAdjustmentDetail.Errors.Add("Item", "Harus Ada");
            }
            return stockAdjustmentDetail;
        }

        public StockAdjustmentDetail VIsNotConfirmed(StockAdjustmentDetail stockAdjustmentDetail)
        {
            if (stockAdjustmentDetail.IsConfirmed)
            {
                stockAdjustmentDetail.Errors.Add("IsConfirmed", "Harus False");
            }
            return stockAdjustmentDetail;
        }

        public StockAdjustmentDetail VIsNotZeroQuantity(StockAdjustmentDetail stockAdjustmentDetail)
        {
            if (stockAdjustmentDetail.Quantity == 0)
            {
                stockAdjustmentDetail.Errors.Add("Quantity", "Tidak boleh 0");
            }
            return stockAdjustmentDetail;
        }

        public StockAdjustmentDetail VIsConfirmQuantityValid(StockAdjustmentDetail stockAdjustmentDetail, IItemService _itemService)
        {
            Item item = _itemService.GetObjectById(stockAdjustmentDetail.ItemId);
            if (item == null || stockAdjustmentDetail.Quantity + item.Quantity < 0)
            {
                stockAdjustmentDetail.Errors.Add("Quantity + Item Quantity", "Harus lebih besar atau sama dengan 0");
            }
            return stockAdjustmentDetail;
        }

        public StockAdjustmentDetail VIsUnconfirmQuantityValid(StockAdjustmentDetail stockAdjustmentDetail, IItemService _itemService)
        {
            Item item = _itemService.GetObjectById(stockAdjustmentDetail.ItemId);
            if (item == null || item.Quantity - stockAdjustmentDetail.Quantity < 0)
            {
                stockAdjustmentDetail.Errors.Add("Item Quantity - Quantity", "Harus lebih besar atau sama dengan 0");
            }
            return stockAdjustmentDetail;
        }

        public StockAdjustmentDetail VIsItemUnique(StockAdjustmentDetail stockAdjustmentDetail, IStockAdjustmentDetailService _stockAdjustmentDetailService)
        {
            IList<StockAdjustmentDetail> stockAdjustmentDetails = _stockAdjustmentDetailService.GetObjectsByItemId(stockAdjustmentDetail.ItemId);
            int same = 0;
            foreach (var d in stockAdjustmentDetails)
            {
                if (d.ItemId == stockAdjustmentDetail.ItemId && d.StockAdjustmentId == stockAdjustmentDetail.StockAdjustmentId && !d.IsDeleted) same++;
            }
            if (same > 0)
            {
                stockAdjustmentDetail.Errors.Add("Item", "Harus unik");
            }
            return stockAdjustmentDetail;
        }

        public StockAdjustmentDetail VCreateObject(StockAdjustmentDetail stockAdjustmentDetail, IStockAdjustmentDetailService _stockAdjustmentDetailService, IStockAdjustmentService _stockAdjustmentService, IItemService _itemService)
        {

            VHasStockAdjustment(stockAdjustmentDetail, _stockAdjustmentService);
            VHasItem(stockAdjustmentDetail, _itemService);
            VIsNotZeroQuantity(stockAdjustmentDetail);
            VIsItemUnique(stockAdjustmentDetail, _stockAdjustmentDetailService);
            return stockAdjustmentDetail;
        }

        public StockAdjustmentDetail VUpdateObject(StockAdjustmentDetail stockAdjustmentDetail, IStockAdjustmentDetailService _stockAdjustmentDetailService, IStockAdjustmentService _stockAdjustmentService, IItemService _itemService)
        {
            VHasStockAdjustment(stockAdjustmentDetail, _stockAdjustmentService);
            VHasItem(stockAdjustmentDetail, _itemService);
            VIsNotZeroQuantity(stockAdjustmentDetail);
            VIsItemUnique(stockAdjustmentDetail, _stockAdjustmentDetailService);
            VIsNotConfirmed(stockAdjustmentDetail);
            return stockAdjustmentDetail;
        }

        public StockAdjustmentDetail VDeleteObject(StockAdjustmentDetail stockAdjustmentDetail, IStockAdjustmentDetailService _stockAdjustmentDetailService)
        {
            VIsNotConfirmed(stockAdjustmentDetail);
            return stockAdjustmentDetail;
        }

        public StockAdjustmentDetail VConfirmObject(StockAdjustmentDetail stockAdjustmentDetail, IItemService _itemService)
        {
            VIsConfirmQuantityValid(stockAdjustmentDetail, _itemService);
            return stockAdjustmentDetail;
        }

        public StockAdjustmentDetail VUnconfirmObject(StockAdjustmentDetail stockAdjustmentDetail, IItemService _itemService)
        {
            VIsUnconfirmQuantityValid(stockAdjustmentDetail, _itemService);
            return stockAdjustmentDetail;
        }

        public bool ValidCreateObject(StockAdjustmentDetail stockAdjustmentDetail, IStockAdjustmentDetailService _stockAdjustmentDetailService, IStockAdjustmentService _stockAdjustmentService, IItemService _itemService)
        {
            VCreateObject(stockAdjustmentDetail, _stockAdjustmentDetailService, _stockAdjustmentService, _itemService);
            return isValid(stockAdjustmentDetail);
        }

        public bool ValidUpdateObject(StockAdjustmentDetail stockAdjustmentDetail, IStockAdjustmentDetailService _stockAdjustmentDetailService, IStockAdjustmentService _stockAdjustmentService, IItemService _itemService)
        {
            stockAdjustmentDetail.Errors.Clear();
            VUpdateObject(stockAdjustmentDetail, _stockAdjustmentDetailService, _stockAdjustmentService, _itemService);
            return isValid(stockAdjustmentDetail);
        }

        public bool ValidDeleteObject(StockAdjustmentDetail stockAdjustmentDetail, IStockAdjustmentDetailService _stockAdjustmentDetailService)
        {
            stockAdjustmentDetail.Errors.Clear();
            VDeleteObject(stockAdjustmentDetail, _stockAdjustmentDetailService);
            return isValid(stockAdjustmentDetail);
        }

        public bool ValidConfirmObject(StockAdjustmentDetail stockAdjustmentDetail, IItemService _itemService)
        {
            stockAdjustmentDetail.Errors.Clear();
            VConfirmObject(stockAdjustmentDetail, _itemService);
            return isValid(stockAdjustmentDetail);
        }

        public bool ValidUnconfirmObject(StockAdjustmentDetail stockAdjustmentDetail, IItemService _itemService)
        {
            stockAdjustmentDetail.Errors.Clear();
            VUnconfirmObject(stockAdjustmentDetail, _itemService);
            return isValid(stockAdjustmentDetail);
        }

        public bool isValid(StockAdjustmentDetail obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }

        public string PrintError(StockAdjustmentDetail obj)
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
