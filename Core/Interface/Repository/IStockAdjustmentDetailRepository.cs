using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Interface.Service;

namespace Core.Interface.Repository
{
    public interface IStockAdjustmentDetailRepository : IRepository<StockAdjustmentDetail>
    {
        IList<StockAdjustmentDetail> GetAll();
        IList<StockAdjustmentDetail> GetObjectsByItemId(int ItemId);
        IList<StockAdjustmentDetail> GetObjectsByStockAdjustmentId(int StockAdjustmentId);
        IList<StockAdjustmentDetail> GetConfirmedObjectsByStockAdjustmentId(int StockAdjustmentId);
        StockAdjustmentDetail GetObjectById(int Id);
        StockAdjustmentDetail CreateObject(StockAdjustmentDetail stockAdjustmentDetail, string ParentCode);
        StockAdjustmentDetail UpdateObject(StockAdjustmentDetail stockAdjustmentDetail);
        StockAdjustmentDetail SoftDeleteObject(StockAdjustmentDetail stockAdjustmentDetail);
        StockAdjustmentDetail ConfirmObject(StockAdjustmentDetail stockAdjustmentDetail, IStockMutationService _stockMutationService, IItemService _itemService);
        StockAdjustmentDetail UnconfirmObject(StockAdjustmentDetail stockAdjustmentDetail, IStockMutationService _stockMutationService, IItemService _itemService);
        bool DeleteObject(int Id);
        string SetObjectCode(StockAdjustmentDetail obj, string ParentCode);
    }
}
