using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using Core.DomainModel;
using Core.Interface.Service;

namespace Validation.Validation
{
    public class StockAdjustmentValidator : IStockAdjustmentValidator
    {
        public StockAdjustment VIsValidAdjustmentDate(StockAdjustment stockAdjustment)
        {
            if (stockAdjustment.AdjustmentDate == null || stockAdjustment.AdjustmentDate.Equals(DateTime.FromBinary(0)))
            {
                stockAdjustment.Errors.Add("AdjustmentDate", "Tidak Valid");
            }
            return stockAdjustment;
        }

        public StockAdjustment VIsConfirmed(StockAdjustment stockAdjustment)
        {
            if (!stockAdjustment.IsConfirmed)
            {
                stockAdjustment.Errors.Add("IsConfirmed", "Harus True");
            }
            return stockAdjustment;
        }

        public StockAdjustment VIsNotConfirmed(StockAdjustment stockAdjustment)
        {
            if (stockAdjustment.IsConfirmed)
            {
                stockAdjustment.Errors.Add("IsConfirmed", "Harus False");
            }
            return stockAdjustment;
        }

        public StockAdjustment VIsStockAdjustmentDetailsNotConfirmed(StockAdjustment stockAdjustment, IStockAdjustmentDetailService _stockAdjustmentDetailService)
        {
            IList<StockAdjustmentDetail> stockAdjustmentDetails = _stockAdjustmentDetailService.GetConfirmedObjectsByStockAdjustmentId(stockAdjustment.Id);
            if (stockAdjustmentDetails.Any())
            {
                stockAdjustment.Errors.Add("StockAdjustmentDetails", "Harus tidak terkonfirmasi semua");
            }
            return stockAdjustment;
        }

        public StockAdjustment VHasStockAdjustmentDetails(StockAdjustment stockAdjustment, IStockAdjustmentDetailService _stockAdjustmentDetailService)
        {
            IList<StockAdjustmentDetail> stockAdjustmentDetails = _stockAdjustmentDetailService.GetObjectsByStockAdjustmentId(stockAdjustment.Id);
            if (!stockAdjustmentDetails.Any())
            {
                stockAdjustment.Errors.Add("StockAdjustmentDetails", "Harus ada");
            }
            return stockAdjustment;
        }

        public StockAdjustment VIsValidStockAdjustmentDetailsQuantity(StockAdjustment stockAdjustment, IStockAdjustmentDetailService _stockAdjustmentDetailService, IItemService _itemService)
        {
            IList<StockAdjustmentDetail> stockAdjustmentDetails = _stockAdjustmentDetailService.GetObjectsByStockAdjustmentId(stockAdjustment.Id);
            bool valid = true;
            foreach (var sad in stockAdjustmentDetails)
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
                stockAdjustment.Errors.Add("StockAdjustmentDetails Quantity + Item Quantity", "Harus lebih besar atau sama dengan 0");
            }
            return stockAdjustment;
        }

        public StockAdjustment VCreateObject(StockAdjustment stockAdjustment)
        {

            VIsValidAdjustmentDate(stockAdjustment);
            return stockAdjustment;
        }

        public StockAdjustment VUpdateObject(StockAdjustment stockAdjustment)
        {
            VIsNotConfirmed(stockAdjustment);
            VIsValidAdjustmentDate(stockAdjustment);
            return stockAdjustment;
        }

        public StockAdjustment VDeleteObject(StockAdjustment stockAdjustment, IStockAdjustmentDetailService _stockAdjustmentDetailService)
        {
            VIsNotConfirmed(stockAdjustment);
            VIsStockAdjustmentDetailsNotConfirmed(stockAdjustment, _stockAdjustmentDetailService);
            return stockAdjustment;
        }

        public StockAdjustment VConfirmObject(StockAdjustment stockAdjustment, IStockAdjustmentDetailService _stockAdjustmentDetailService, IItemService _itemService)
        {
            VHasStockAdjustmentDetails(stockAdjustment, _stockAdjustmentDetailService);
            VIsValidStockAdjustmentDetailsQuantity(stockAdjustment, _stockAdjustmentDetailService, _itemService);
            VIsNotConfirmed(stockAdjustment);
            return stockAdjustment;
        }

        public StockAdjustment VUnconfirmObject(StockAdjustment stockAdjustment, IStockAdjustmentDetailService _stockAdjustmentDetailService, IItemService _itemService)
        {
            VIsConfirmed(stockAdjustment);
            VIsValidStockAdjustmentDetailsQuantity(stockAdjustment, _stockAdjustmentDetailService, _itemService);
            return stockAdjustment;
        }

        public bool ValidCreateObject(StockAdjustment stockAdjustment)
        {
            VCreateObject(stockAdjustment);
            return isValid(stockAdjustment);
        }

        public bool ValidUpdateObject(StockAdjustment stockAdjustment)
        {
            stockAdjustment.Errors.Clear();
            VUpdateObject(stockAdjustment);
            return isValid(stockAdjustment);
        }

        public bool ValidDeleteObject(StockAdjustment stockAdjustment, IStockAdjustmentDetailService _stockAdjustmentDetailService)
        {
            stockAdjustment.Errors.Clear();
            VDeleteObject(stockAdjustment, _stockAdjustmentDetailService);
            return isValid(stockAdjustment);
        }

        public bool ValidConfirmObject(StockAdjustment stockAdjustment, IStockAdjustmentDetailService _stockAdjustmentDetailService, IItemService _itemService)
        {
            stockAdjustment.Errors.Clear();
            VConfirmObject(stockAdjustment, _stockAdjustmentDetailService, _itemService);
            return isValid(stockAdjustment);
        }

        public bool ValidUnconfirmObject(StockAdjustment stockAdjustment, IStockAdjustmentDetailService _stockAdjustmentDetailService, IItemService _itemService)
        {
            stockAdjustment.Errors.Clear();
            VUnconfirmObject(stockAdjustment, _stockAdjustmentDetailService, _itemService);
            return isValid(stockAdjustment);
        }

        public bool isValid(StockAdjustment obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }

        public string PrintError(StockAdjustment obj)
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
