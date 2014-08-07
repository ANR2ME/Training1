using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Interface.Service;

namespace Core.Interface.Repository
{
    public interface IStockAdjustmentRepository : IRepository<StockAdjustment>
    {
        IList<StockAdjustment> GetAll();
        StockAdjustment GetObjectById(int Id);
        StockAdjustment CreateObject(StockAdjustment stockAdjustment);
        StockAdjustment UpdateObject(StockAdjustment stockAdjustment);
        StockAdjustment SoftDeleteObject(StockAdjustment stockAdjustment);
        StockAdjustment ConfirmObject(StockAdjustment stockAdjustment, IStockAdjustmentDetailService _stockAdjustmentDetailService, IStockMutationService _stockMutationService, IItemService _itemService);
        StockAdjustment UnconfirmObject(StockAdjustment stockAdjustment, IStockAdjustmentDetailService _stockAdjustmentDetailService, IStockMutationService _stockMutationService, IItemService _itemService);
        bool DeleteObject(int Id);
        string SetObjectCode(StockAdjustment obj);
    }
}
