using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DomainModel;
using Validation.Validation;

namespace Core.Interface.Service
{
    public interface IStockAdjustmentService
    {
        IStockAdjustmentValidator GetValidator();
        IList<StockAdjustment> GetAll();

        StockAdjustment GetObjectById(int Id);
        StockAdjustment CreateObject(StockAdjustment stockAdjustment);
        StockAdjustment UpdateObject(StockAdjustment stockAdjustment);
        StockAdjustment ConfirmObject(StockAdjustment stockAdjustment, IStockAdjustmentDetailService _stockAdjustmentDetailService, IStockMutationService _stockMutationService, IItemService _itemService);
        StockAdjustment UnconfirmObject(StockAdjustment stockAdjustment, IStockAdjustmentDetailService _stockAdjustmentDetailService, IStockMutationService _stockMutationService, IItemService _itemService);
        StockAdjustment SoftDeleteObject(StockAdjustment stockAdjustment, IStockAdjustmentDetailService _stockAdjustmentDetailService);
        bool DeleteObject(int Id);
    }
}
