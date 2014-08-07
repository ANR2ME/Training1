using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DomainModel;
using Core.Interface.Service;

namespace Core.Interface.Validation
{
    public interface IStockAdjustmentDetailValidator
    {
        StockAdjustmentDetail VHasStockAdjustment(StockAdjustmentDetail stockAdjustmentDetail);
        StockAdjustmentDetail VHasItem(StockAdjustmentDetail stockAdjustmentDetail);
        StockAdjustmentDetail VIsNotConfirmed(StockAdjustmentDetail stockAdjustmentDetail);
        StockAdjustmentDetail VIsNotZeroQuantity(StockAdjustmentDetail stockAdjustmentDetail);
        StockAdjustmentDetail VIsConfirmQuantityValid(StockAdjustmentDetail stockAdjustmentDetail, IItemService _itemService);
        StockAdjustmentDetail VIsUnconfirmQuantityValid(StockAdjustmentDetail stockAdjustmentDetail, IItemService _itemService);
        StockAdjustmentDetail VIsItemUnique(StockAdjustmentDetail stockAdjustmentDetail, IStockAdjustmentDetailService _stockAdjustmentDetailService);
        StockAdjustmentDetail VCreateObject(StockAdjustmentDetail stockAdjustmentDetail, IStockAdjustmentDetailService _stockAdjustmentDetailService);
        StockAdjustmentDetail VUpdateObject(StockAdjustmentDetail stockAdjustmentDetail, IStockAdjustmentDetailService _stockAdjustmentDetailService);
        StockAdjustmentDetail VDeleteObject(StockAdjustmentDetail stockAdjustmentDetail, IStockAdjustmentDetailService _stockAdjustmentDetailService);
        StockAdjustmentDetail VUnconfirmObject(StockAdjustmentDetail stockAdjustmentDetail, IItemService _itemService);

        bool ValidCreateObject(StockAdjustmentDetail stockAdjustmentDetail, IStockAdjustmentDetailService _stockAdjustmentDetailService);
        bool ValidUpdateObject(StockAdjustmentDetail stockAdjustmentDetail, IStockAdjustmentDetailService _stockAdjustmentDetailService);
        bool ValidDeleteObject(StockAdjustmentDetail stockAdjustmentDetail, IStockAdjustmentDetailService _stockAdjustmentDetailService);
        bool ValidConfirmObject(StockAdjustmentDetail stockAdjustmentDetail, IItemService _itemService);
        bool ValidUnconfirmObject(StockAdjustmentDetail stockAdjustmentDetail, IItemService _itemService);
        bool isValid(StockAdjustmentDetail obj);
        string PrintError(StockAdjustmentDetail obj);
    }
}
