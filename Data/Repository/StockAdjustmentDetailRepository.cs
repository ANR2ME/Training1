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
    public class StockAdjustmentDetailRepository : EfRepository<StockAdjustmentDetail>, IStockAdjustmentDetailRepository
    {

        private StockControlEntities entities;
        public StockAdjustmentDetailRepository()
        {
            entities = new StockControlEntities();
        }

        public IList<StockAdjustmentDetail> GetAll()
        {
            return FindAll().ToList();
        }

        public IList<StockAdjustmentDetail> GetObjectsByItemId(int ItemId)
        {
            return FindAll(x => x.ItemId == ItemId && !x.IsDeleted).ToList();
        }

        public IList<StockAdjustmentDetail> GetObjectsByStockAdjustmentId(int StockAdjustmentId)
        {
            return FindAll(x => x.StockAdjustmentId == StockAdjustmentId && !x.IsDeleted).ToList();
        }

        public IList<StockAdjustmentDetail> GetConfirmedObjectsByStockAdjustmentId(int StockAdjustmentId)
        {
            return FindAll(x => x.StockAdjustmentId == StockAdjustmentId && x.IsConfirmed && !x.IsDeleted).ToList();
        }

        public StockAdjustmentDetail GetObjectById(int Id)
        {
            StockAdjustmentDetail sad = Find(x => x.Id == Id && !x.IsDeleted);
            if (sad != null) { sad.Errors = new Dictionary<string, string>(); }
            return sad;
        }

        public StockAdjustmentDetail CreateObject(StockAdjustmentDetail stockAdjustmentDetail, string ParentCode)
        {
            stockAdjustmentDetail.IsDeleted = false;
            stockAdjustmentDetail.CreatedAt = DateTime.Now;
            stockAdjustmentDetail.IsConfirmed = false;
            stockAdjustmentDetail.Code = SetObjectCode(stockAdjustmentDetail, ParentCode);
            return Create(stockAdjustmentDetail);
        }

        public StockAdjustmentDetail UpdateObject(StockAdjustmentDetail stockAdjustmentDetail)
        {
            stockAdjustmentDetail.UpdatedAt = DateTime.Now;
            Update(stockAdjustmentDetail);
            return stockAdjustmentDetail;
        }

        public StockAdjustmentDetail SoftDeleteObject(StockAdjustmentDetail stockAdjustmentDetail)
        {
            stockAdjustmentDetail.IsDeleted = true;
            stockAdjustmentDetail.DeletedAt = DateTime.Now;
            Update(stockAdjustmentDetail);
            return stockAdjustmentDetail;
        }

        public StockAdjustmentDetail ConfirmObject(StockAdjustmentDetail stockAdjustmentDetail, IStockMutationService _stockMutationService, IItemService _itemService)
        {
            StockMutation sm = new StockMutation()
            {
                ItemId = stockAdjustmentDetail.ItemId,
                Status = (stockAdjustmentDetail.Quantity >= 0) ? "Addition" : "Deduction",
                ItemCase = "Ready",
                Quantity = Math.Abs(stockAdjustmentDetail.Quantity),
                SourceDocumentType = "StockAdjustment",
                SourceDocumentId = stockAdjustmentDetail.StockAdjustmentId,
                SourceDocumentDetailType = "StockAdjustmentDetail",
                SourceDocumentDetailId = stockAdjustmentDetail.Id,
            };
            _stockMutationService.CreateObject(sm);
            _stockMutationService.StockMutateObject(sm, _itemService);
            stockAdjustmentDetail.IsConfirmed = true;
            stockAdjustmentDetail.ConfirmationDate = DateTime.Now;
            Update(stockAdjustmentDetail);
            return stockAdjustmentDetail;
        }

        public StockAdjustmentDetail UnconfirmObject(StockAdjustmentDetail stockAdjustmentDetail, IStockMutationService _stockMutationService, IItemService _itemService)
        {
            IList<StockMutation> smlist = _stockMutationService.GetObjectsByAllIds(stockAdjustmentDetail.ItemId, stockAdjustmentDetail.Id, "StockAdjustmentDetail");
            foreach (var sm in smlist)
            {
                _stockMutationService.ReverseStockMutateObject(sm, _itemService);
                _stockMutationService.SoftDeleteObject(sm);
            }
            stockAdjustmentDetail.IsConfirmed = false;
            stockAdjustmentDetail.ConfirmationDate = null;
            stockAdjustmentDetail.UpdatedAt = DateTime.Now;
            Update(stockAdjustmentDetail);
            return stockAdjustmentDetail;
        }

        public bool DeleteObject(int Id)
        {
            StockAdjustmentDetail sad = Find(x => x.Id == Id);
            return (Delete(sad) == 1) ? true : false;
        }

        public string SetObjectCode(StockAdjustmentDetail obj, string ParentCode)
        {
            // Code = #{StockAdjustment.Code}/#{totalnumber + 1}
            int totalobject = FindAll(x => x.CreatedAt.Year == DateTime.Now.Year).Count() + 1;
            string Code = ParentCode + "/" + totalobject;
            return Code;
        }
    }
}
