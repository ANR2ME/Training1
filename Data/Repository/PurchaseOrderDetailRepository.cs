using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DomainModel;
using Core.Interface.Service;
using Data.Context;

namespace Data.Repository
{
    public class PurchaseOrderDetailRepository : EfRepository<PurchaseOrderDetail>, IPurchaseOrderDetailRepository
    {
        private StockControlEntities entities;
        public PurchaseOrderDetailRepository()
        {
            entities = new StockControlEntities();
        }

        public IList<PurchaseOrderDetail> GetAll()
        {
            return FindAll().ToList();
        }

        public IList<PurchaseOrderDetail> GetObjectsByItemId(int ItemId)
        {
            return FindAll(x => x.ItemId == ItemId && !x.IsDeleted).ToList();
        }

        public IList<PurchaseOrderDetail> GetObjectsByPurchaseOrderId(int PurchaseOrderId)
        {
            return FindAll(x => x.PurchaseOrderId == PurchaseOrderId && !x.IsDeleted).ToList();
        }

        public IList<PurchaseOrderDetail> GetConfirmedObjectsByPurchaseOrderId(int PurchaseOrderId)
        {
            return FindAll(x => x.PurchaseOrderId == PurchaseOrderId && x.IsConfirmed && !x.IsDeleted).ToList();
        }

        public PurchaseOrderDetail GetObjectById(int Id)
        {
            PurchaseOrderDetail pod = Find(x => x.Id == Id && !x.IsDeleted);
            if (pod != null) { pod.Errors = new Dictionary<string, string>(); }
            return pod;
        }

        public PurchaseOrderDetail CreateObject(PurchaseOrderDetail purchaseOrderDetail, string ParentCode)
        {
            purchaseOrderDetail.CreatedAt = DateTime.Now;
            purchaseOrderDetail.IsDeleted = false;
            purchaseOrderDetail.IsConfirmed = false;
            purchaseOrderDetail.Code = SetObjectCode(purchaseOrderDetail, ParentCode);
            return Create(purchaseOrderDetail);
        }

        public PurchaseOrderDetail UpdateObject(PurchaseOrderDetail purchaseOrderDetail)
        {
            purchaseOrderDetail.UpdatedAt = DateTime.Now;
            Update(purchaseOrderDetail);
            return purchaseOrderDetail;
        }

        public PurchaseOrderDetail SoftDeleteObject(PurchaseOrderDetail purchaseOrderDetail)
        {
            purchaseOrderDetail.IsDeleted = true;
            purchaseOrderDetail.DeletedAt = DateTime.Now;
            Update(purchaseOrderDetail);
            return purchaseOrderDetail;
        }

        public PurchaseOrderDetail ConfirmObject(PurchaseOrderDetail purchaseOrderDetail, IStockMutationService _stockMutationService, IItemService _itemService)
        {
            StockMutation sm = new StockMutation()
            {
                ItemId = purchaseOrderDetail.ItemId,
                Status = "Addition",
                ItemCase = "PendingReceival",
                Quantity = purchaseOrderDetail.Quantity,
                SourceDocumentType = "PurchaseOrder",
                SourceDocumentId = purchaseOrderDetail.PurchaseOrderId,
                SourceDocumentDetailType = "PurchaseOrderDetail",
                SourceDocumentDetailId = purchaseOrderDetail.Id,
            };
            _stockMutationService.CreateObject(sm);
            _stockMutationService.StockMutateObject(sm, _itemService);
            purchaseOrderDetail.IsConfirmed = true;
            purchaseOrderDetail.ConfirmationDate = DateTime.Now;
            Update(purchaseOrderDetail);
            return purchaseOrderDetail;
        }

        public PurchaseOrderDetail UnconfirmObject(PurchaseOrderDetail purchaseOrderDetail, IStockMutationService _stockMutationService, IItemService _itemService)
        {
            IList<StockMutation> smlist = _stockMutationService.GetObjectsByAllIds(purchaseOrderDetail.ItemId, purchaseOrderDetail.Id, "PurchaseOrderDetail");
            foreach (var sm in smlist)
            {
                _stockMutationService.ReverseStockMutateObject(sm, _itemService);
                _stockMutationService.SoftDeleteObject(sm);
            }
            purchaseOrderDetail.IsConfirmed = false;
            purchaseOrderDetail.ConfirmationDate = null;
            purchaseOrderDetail.UpdatedAt = DateTime.Now;
            Update(purchaseOrderDetail);
            return purchaseOrderDetail;
        }

        public bool DeleteObject(int Id)
        {
            PurchaseOrderDetail pod = Find(x => x.Id == Id);
            return (Delete(pod) == 1) ? true : false;
        }

        public string SetObjectCode(PurchaseOrderDetail obj, string ParentCode)
        {
            // Code = #{PurchaseOrder.Code}/#{totalnumber + 1}
            int totalobject = FindAll(x => x.CreatedAt.Year == DateTime.Now.Year).Count() + 1;
            string Code = ParentCode + "/" + totalobject;
            return Code;
        }
    }
}
