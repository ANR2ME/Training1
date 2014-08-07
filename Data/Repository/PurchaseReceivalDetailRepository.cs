using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DomainModel;
using Core.Interface.Repository;
using Core.Interface.Service;
using Data.Context;

namespace Data.Repository
{
    public class PurchaseReceivalDetailRepository : EfRepository<PurchaseReceivalDetail>, IPurchaseReceivalDetailRepository
    {
        private StockControlEntities entities;
        public PurchaseReceivalDetailRepository()
        {
            entities = new StockControlEntities();
        }

        public IList<PurchaseReceivalDetail> GetAll()
        {
            return FindAll().ToList();
        }

        public IList<PurchaseReceivalDetail> GetObjectsByItemId(int ItemId)
        {
            return FindAll(x => x.ItemId == ItemId && !x.IsDeleted).ToList();
        }

        public IList<PurchaseReceivalDetail> GetObjectsByPurchaseReceivalId(int PurchaseReceivalId)
        {
            return FindAll(x => x.PurchaseReceivalId == PurchaseReceivalId && !x.IsDeleted).ToList();
        }

        public IList<PurchaseReceivalDetail> GetConfirmedObjectsByPurchaseReceivalId(int PurchaseReceivalId)
        {
            return FindAll(x => x.PurchaseReceivalId == PurchaseReceivalId && x.IsConfirmed && !x.IsDeleted).ToList();
        }

        public IList<PurchaseReceivalDetail> GetObjectsByPurchaseOrderDetailId(int PurchaseOrderDetailId)
        {
            return FindAll(x => x.PurchaseOrderDetailId == PurchaseOrderDetailId && !x.IsDeleted).ToList();
        }

        public PurchaseReceivalDetail GetObjectById(int Id)
        {
            PurchaseReceivalDetail prd = Find(x => x.Id == Id && !x.IsDeleted);
            if (prd != null) { prd.Errors = new Dictionary<string, string>(); }
            return prd;
        }

        public PurchaseReceivalDetail CreateObject(PurchaseReceivalDetail purchaseReceivalDetail, string ParentCode)
        {
            purchaseReceivalDetail.CreatedAt = DateTime.Now;
            purchaseReceivalDetail.IsDeleted = false;
            purchaseReceivalDetail.IsConfirmed = false;
            purchaseReceivalDetail.Code = SetObjectCode(purchaseReceivalDetail, ParentCode);
            return Create(purchaseReceivalDetail);
        }

        public PurchaseReceivalDetail UpdateObject(PurchaseReceivalDetail purchaseReceivalDetail)
        {
            purchaseReceivalDetail.UpdatedAt = DateTime.Now;
            Update(purchaseReceivalDetail);
            return purchaseReceivalDetail;
        }

        public PurchaseReceivalDetail SoftDeleteObject(PurchaseReceivalDetail purchaseReceivalDetail)
        {
            purchaseReceivalDetail.IsDeleted = true;
            purchaseReceivalDetail.DeletedAt = DateTime.Now;
            Update(purchaseReceivalDetail);
            return purchaseReceivalDetail;
        }

        public PurchaseReceivalDetail ConfirmObject(PurchaseReceivalDetail purchaseReceivalDetail, IStockMutationService _stockMutationService, IItemService _itemService)
        {
            StockMutation sm = new StockMutation()
            {
                ItemId = purchaseReceivalDetail.ItemId,
                Status = "Deduction",
                ItemCase = "PendingReceival",
                Quantity = purchaseReceivalDetail.Quantity,
                SourceDocumentType = "PurchaseReceival",
                SourceDocumentId = purchaseReceivalDetail.PurchaseReceivalId,
                SourceDocumentDetailType = "PurchaseReceivalDetail",
                SourceDocumentDetailId = purchaseReceivalDetail.Id,
            };
            _stockMutationService.CreateObject(sm);
            _stockMutationService.StockMutateObject(sm, _itemService);
            StockMutation sm2 = new StockMutation()
            {
                ItemId = purchaseReceivalDetail.ItemId,
                Status = "Addition",
                ItemCase = "Ready",
                Quantity = purchaseReceivalDetail.Quantity,
                SourceDocumentType = "PurchaseReceival",
                SourceDocumentId = purchaseReceivalDetail.PurchaseReceivalId,
                SourceDocumentDetailType = "PurchaseReceivalDetail",
                SourceDocumentDetailId = purchaseReceivalDetail.Id,
            };
            _stockMutationService.CreateObject(sm2);
            _stockMutationService.StockMutateObject(sm2, _itemService);
            purchaseReceivalDetail.IsConfirmed = true;
            purchaseReceivalDetail.ConfirmationDate = DateTime.Now;
            Update(purchaseReceivalDetail);
            return purchaseReceivalDetail;
        }

        public PurchaseReceivalDetail UnconfirmObject(PurchaseReceivalDetail purchaseReceivalDetail, IStockMutationService _stockMutationService, IItemService _itemService)
        {
            IList<StockMutation> smlist = _stockMutationService.GetObjectsByAllIds(purchaseReceivalDetail.ItemId, purchaseReceivalDetail.Id, "PurchaseReceivalDetail");
            foreach (var sm in smlist)
            {
                _stockMutationService.ReverseStockMutateObject(sm, _itemService);
                _stockMutationService.SoftDeleteObject(sm);
            }
            purchaseReceivalDetail.IsConfirmed = false;
            purchaseReceivalDetail.ConfirmationDate = null;
            purchaseReceivalDetail.UpdatedAt = DateTime.Now;
            Update(purchaseReceivalDetail);
            return purchaseReceivalDetail;
        }

        public bool DeleteObject(int Id)
        {
            PurchaseReceivalDetail prd = Find(x => x.Id == Id);
            return (Delete(prd) == 1) ? true : false;
        }

        public string SetObjectCode(PurchaseReceivalDetail obj, string ParentCode)
        {
            // Code = #{PurchaseOrder.Code}/#{totalnumber + 1}
            int totalobject = FindAll(x => x.CreatedAt.Year == DateTime.Now.Year).Count() + 1;
            string Code = ParentCode + "/" + totalobject;
            return Code;
        }
    }
}
