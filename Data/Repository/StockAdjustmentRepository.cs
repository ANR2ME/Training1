using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DomainModel;
using Core.Interface.Repository;
using Data.Context;
using Data.Repository;
using System.Data;
using Core.Interface.Service;

namespace Data.Repository
{
    public class StockAdjustmentRepository : EfRepository<StockAdjustment>, IStockAdjustmentRepository
    {

        private StockControlEntities entities;
        public StockAdjustmentRepository()
        {
            entities = new StockControlEntities();
        }

        public IList<StockAdjustment> GetAll()
        {
            return FindAll().ToList();
        }

        public StockAdjustment GetObjectById(int Id)
        {
            StockAdjustment sa = Find(x => x.Id == Id && !x.IsDeleted);
            if (sa != null) { sa.Errors = new Dictionary<string, string>(); }
            return sa;
        }

        public StockAdjustment CreateObject(StockAdjustment stockAdjustment)
        {
            stockAdjustment.IsDeleted = false;
            stockAdjustment.CreatedAt = DateTime.Now;
            stockAdjustment.IsConfirmed = false;
            stockAdjustment.Code = SetObjectCode(stockAdjustment);
            return Create(stockAdjustment);
        }

        public StockAdjustment UpdateObject(StockAdjustment stockAdjustment)
        {
            stockAdjustment.UpdatedAt = DateTime.Now;
            Update(stockAdjustment);
            return stockAdjustment;
        }

        public StockAdjustment SoftDeleteObject(StockAdjustment stockAdjustment)
        {
            stockAdjustment.IsDeleted = true;
            stockAdjustment.DeletedAt = DateTime.Now;
            Update(stockAdjustment);
            return stockAdjustment;
        }

        public StockAdjustment ConfirmObject(StockAdjustment stockAdjustment, IStockAdjustmentDetailService _stockAdjustmentDetailService, IStockMutationService _stockMutationService, IItemService _itemService)
        {
            stockAdjustment.IsConfirmed = true;
            stockAdjustment.ConfirmationDate = DateTime.Now;
            Update(stockAdjustment);
            IList<StockAdjustmentDetail> stockAdjustmentDetails = _stockAdjustmentDetailService.GetObjectsByStockAdjustmentId(stockAdjustment.Id);
            foreach (var sad in stockAdjustmentDetails)
            {
                _stockAdjustmentDetailService.ConfirmObject(sad, _stockMutationService, _itemService);
            }
            return stockAdjustment;
        }

        public StockAdjustment UnconfirmObject(StockAdjustment stockAdjustment, IStockAdjustmentDetailService _stockAdjustmentDetailService, IStockMutationService _stockMutationService, IItemService _itemService)
        {
            stockAdjustment.IsConfirmed = false;
            stockAdjustment.ConfirmationDate = null;
            stockAdjustment.UpdatedAt = DateTime.Now;
            Update(stockAdjustment);
            IList<StockAdjustmentDetail> stockAdjustmentDetails = _stockAdjustmentDetailService.GetObjectsByStockAdjustmentId(stockAdjustment.Id);
            foreach (var sad in stockAdjustmentDetails)
            {
                _stockAdjustmentDetailService.UnconfirmObject(sad, _stockMutationService, _itemService);
            }
            return stockAdjustment;
        }

        public bool DeleteObject(int Id)
        {
            StockAdjustment sa = Find(x => x.Id == Id);
            return (Delete(sa) == 1) ? true : false;
        }

        public string SetObjectCode(StockAdjustment obj)
        {
            // Code = #{currentyear}/#{totalnumber + 1}
            int totalobject = FindAll(x => x.CreatedAt.Year == DateTime.Now.Year).Count() + 1;
            string Code = DateTime.Now.Year.ToString() + "/" + totalobject;
            return Code;
        }
    }
}
