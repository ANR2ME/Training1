using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DomainModel;
using Core.Interface.Validation;

namespace Core.Interface.Service
{
    public interface IStockAdjustmentDetailService
    {
        IStockAdjustmentDetailValidator GetValidator();
        IList<StockAdjustmentDetail> GetAll();
        IList<StockAdjustmentDetail> GetObjectsByItemId(int ItemId);
        IList<StockAdjustmentDetail> GetObjectsByStockAdjustmentId(int StockAdjustmentId);
        IList<StockAdjustmentDetail> GetConfirmedObjectsByStockAdjustmentId(int StockAdjustmentId);
        StockAdjustmentDetail GetObjectById(int Id);
        StockAdjustmentDetail CreateObject(StockAdjustmentDetail stockAdjustmentDetail, IStockAdjustmentService _stockAdjustmentService, IItemService _itemService);
        StockAdjustmentDetail UpdateObject(StockAdjustmentDetail stockAdjustmentDetail, IStockAdjustmentService _stockAdjustmentService, IItemService _itemService);
        StockAdjustmentDetail SoftDeleteObject(StockAdjustmentDetail stockAdjustmentDetail);
        StockAdjustmentDetail ConfirmObject(StockAdjustmentDetail stockAdjustmentDetail, IStockMutationService _stockMutationService, IItemService _itemService);
        StockAdjustmentDetail UnconfirmObject(StockAdjustmentDetail stockAdjustmentDetail, IStockMutationService _stockMutationService, IItemService _itemService);
        bool DeleteObject(int Id);
    }
}
