using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.DomainModel;
using Core.Interface.Service;

namespace Validation.Validation
{
    public interface IStockAdjustmentValidator
    {
        StockAdjustment VIsValidAdjustmentDate(StockAdjustment stockAdjustment);
        StockAdjustment VIsConfirmed(StockAdjustment stockAdjustment);
        StockAdjustment VIsNotConfirmed(StockAdjustment stockAdjustment);
        StockAdjustment VIsStockAdjustmentDetailsNotConfirmed(StockAdjustment stockAdjustment, IStockAdjustmentDetailService _stockAdjustmentDetailService);

        StockAdjustment VConfirmObject(StockAdjustment stockAdjustment, IStockAdjustmentDetailService _stockAdjustmentDetailService, IItemService _itemService);
        StockAdjustment VUnconfirmObject(StockAdjustment stockAdjustment, IStockAdjustmentDetailService _stockAdjustmentDetailService, IItemService _itemService);
        StockAdjustment VCreateObject(StockAdjustment stockAdjustment);
        StockAdjustment VUpdateObject(StockAdjustment stockAdjustment);
        StockAdjustment VDeleteObject(StockAdjustment stockAdjustment, IStockAdjustmentDetailService _stockAdjustmentDetailService);

        bool ValidCreateObject(StockAdjustment stockAdjustment);
        bool ValidUpdateObject(StockAdjustment stockAdjustment);
        bool ValidDeleteObject(StockAdjustment stockAdjustment, IStockAdjustmentDetailService _stockAdjustmentDetailService);
        bool ValidConfirmObject(StockAdjustment stockAdjustment, IStockAdjustmentDetailService _stockAdjustmentDetailService, IItemService _itemService);
        bool ValidUnconfirmObject(StockAdjustment stockAdjustment, IStockAdjustmentDetailService _stockAdjustmentDetailService, IItemService _itemService);
        bool isValid(StockAdjustment obj);
        string PrintError(StockAdjustment obj);
    }
}
