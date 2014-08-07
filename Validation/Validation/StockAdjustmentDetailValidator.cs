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
        public StockAdjustmentDetail VHasStockAdjustment(StockAdjustmentDetail stockAdjustmentDetail)
        {
            if (stockAdjustmentDetail.StockAdjustmentId <= 0)
            {
                stockAdjustmentDetail.Errors.Add("StockAdjustment", "Harus Ada");
            }
            return stockAdjustmentDetail;
        }

        public StockAdjustmentDetail VHasItem(StockAdjustmentDetail stockAdjustmentDetail)
        {
            if (stockAdjustmentDetail.ItemId <= 0)
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
            if (stockAdjustmentDetail.Quantity + item.Quantity < 0)
            {
                stockAdjustmentDetail.Errors.Add("Quantity + Item Quantity", "Harus lebih besar atau sama dengan 0");
            }
            return stockAdjustmentDetail;
        }

        public StockAdjustmentDetail VIsUnconfirmQuantityValid(StockAdjustmentDetail stockAdjustmentDetail, IItemService _itemService)
        {
            Item item = _itemService.GetObjectById(stockAdjustmentDetail.ItemId);
            if (item.Quantity - stockAdjustmentDetail.Quantity < 0)
            {
                stockAdjustmentDetail.Errors.Add("Item Quantity - Quantity", "Harus lebih besar atau sama dengan 0");
            }
            return stockAdjustmentDetail;
        }

        public StockAdjustmentDetail VIsItemUnique(StockAdjustmentDetail stockAdjustmentDetail, IStockAdjustmentDetailService _stockAdjustmentDetailService)
        {
            IList<StockAdjustmentDetail> stockAdjustmentDetails = _stockAdjustmentDetailService.GetObjectsByStockAdjustmentId(stockAdjustmentDetail.StockAdjustmentId);
            bool unique = true;
            foreach (var sad in stockAdjustmentDetails)
            {
                if (sad.ItemId == stockAdjustmentDetail.ItemId && sad.Id != stockAdjustmentDetail.Id && !sad.IsDeleted)
                {
                    unique = false;
                    break;
                }
            }
            if (!unique)
            {
                stockAdjustmentDetail.Errors.Add("Item", "Harus unik");
            }
            return stockAdjustmentDetail;
        }

        public StockAdjustmentDetail VCreateObject(StockAdjustmentDetail stockAdjustmentDetail, IStockAdjustmentDetailService _stockAdjustmentDetailService)
        {

            VHasStockAdjustment(stockAdjustmentDetail);
            VHasItem(stockAdjustmentDetail);
            VIsNotZeroQuantity(stockAdjustmentDetail);
            VIsItemUnique(stockAdjustmentDetail, _stockAdjustmentDetailService);
            return stockAdjustmentDetail;
        }

        public StockAdjustmentDetail VUpdateObject(StockAdjustmentDetail stockAdjustmentDetail, IStockAdjustmentDetailService _stockAdjustmentDetailService)
        {
            VHasStockAdjustment(stockAdjustmentDetail);
            VHasItem(stockAdjustmentDetail);
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

        public bool ValidCreateObject(StockAdjustmentDetail stockAdjustmentDetail, IStockAdjustmentDetailService _stockAdjustmentDetailService)
        {
            VCreateObject(stockAdjustmentDetail, _stockAdjustmentDetailService);
            return isValid(stockAdjustmentDetail);
        }

        public bool ValidUpdateObject(StockAdjustmentDetail stockAdjustmentDetail, IStockAdjustmentDetailService _stockAdjustmentDetailService)
        {
            stockAdjustmentDetail.Errors.Clear();
            VUpdateObject(stockAdjustmentDetail, _stockAdjustmentDetailService);
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
