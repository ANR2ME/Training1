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
    public class DeliveryOrderDetailRepository : EfRepository<DeliveryOrderDetail>, IDeliveryOrderDetailRepository
    {
        private StockControlEntities entities;
        public DeliveryOrderDetailRepository()
        {
            entities = new StockControlEntities();
        }

        public IList<DeliveryOrderDetail> GetAll()
        {
            return FindAll().ToList();
        }

        public IList<DeliveryOrderDetail> GetObjectsByItemId(int ItemId)
        {
            return FindAll(x => x.ItemId == ItemId && !x.IsDeleted).ToList();
        }

        public IList<DeliveryOrderDetail> GetObjectsByDeliveryOrderId(int DeliveryOrderId)
        {
            return FindAll(x => x.DeliveryOrderId == DeliveryOrderId && !x.IsDeleted).ToList();
        }

        public IList<DeliveryOrderDetail> GetConfirmedObjectsByDeliveryOrderId(int DeliveryOrderId)
        {
            return FindAll(x => x.DeliveryOrderId == DeliveryOrderId && x.IsConfirmed && !x.IsDeleted).ToList();
        }

        public IList<DeliveryOrderDetail> GetObjectsBySalesOrderDetailId(int SalesOrderDetailId)
        {
            return FindAll(x => x.SalesOrderDetailId == SalesOrderDetailId && !x.IsDeleted).ToList();
        }

        public DeliveryOrderDetail GetObjectById(int Id)
        {
            DeliveryOrderDetail prd = Find(x => x.Id == Id && !x.IsDeleted);
            if (prd != null) { prd.Errors = new Dictionary<string, string>(); }
            return prd;
        }

        public DeliveryOrderDetail CreateObject(DeliveryOrderDetail deliveryOrderDetail, string ParentCode)
        {
            deliveryOrderDetail.CreatedAt = DateTime.Now;
            deliveryOrderDetail.IsDeleted = false;
            deliveryOrderDetail.IsConfirmed = false;
            deliveryOrderDetail.Code = SetObjectCode(deliveryOrderDetail, ParentCode);
            return Create(deliveryOrderDetail);
        }

        public DeliveryOrderDetail UpdateObject(DeliveryOrderDetail deliveryOrderDetail)
        {
            deliveryOrderDetail.UpdatedAt = DateTime.Now;
            Update(deliveryOrderDetail);
            return deliveryOrderDetail;
        }

        public DeliveryOrderDetail SoftDeleteObject(DeliveryOrderDetail deliveryOrderDetail)
        {
            deliveryOrderDetail.IsDeleted = true;
            deliveryOrderDetail.DeletedAt = DateTime.Now;
            Update(deliveryOrderDetail);
            return deliveryOrderDetail;
        }

        public DeliveryOrderDetail ConfirmObject(DeliveryOrderDetail deliveryOrderDetail, IStockMutationService _stockMutationService, IItemService _itemService)
        {
            StockMutation sm = new StockMutation()
            {
                ItemId = deliveryOrderDetail.ItemId,
                Status = "Deduction",
                ItemCase = "PendingDelivery",
                Quantity = deliveryOrderDetail.Quantity,
                SourceDocumentType = "DeliveryOrder",
                SourceDocumentId = deliveryOrderDetail.DeliveryOrderId,
                SourceDocumentDetailType = "DeliveryOrderDetail",
                SourceDocumentDetailId = deliveryOrderDetail.Id,
            };
            _stockMutationService.CreateObject(sm, _itemService);
            _stockMutationService.StockMutateObject(sm, _itemService);
            StockMutation sm2 = new StockMutation()
            {
                ItemId = deliveryOrderDetail.ItemId,
                Status = "Deduction",
                ItemCase = "Ready",
                Quantity = deliveryOrderDetail.Quantity,
                SourceDocumentType = "DeliveryOrder",
                SourceDocumentId = deliveryOrderDetail.DeliveryOrderId,
                SourceDocumentDetailType = "DeliveryOrderDetail",
                SourceDocumentDetailId = deliveryOrderDetail.Id,
            };
            _stockMutationService.CreateObject(sm2, _itemService);
            _stockMutationService.StockMutateObject(sm2, _itemService);
            deliveryOrderDetail.IsConfirmed = true;
            deliveryOrderDetail.ConfirmationDate = DateTime.Now;
            Update(deliveryOrderDetail);
            return deliveryOrderDetail;
        }

        public DeliveryOrderDetail UnconfirmObject(DeliveryOrderDetail deliveryOrderDetail, IStockMutationService _stockMutationService, IItemService _itemService)
        {
            IList<StockMutation> smlist = _stockMutationService.GetObjectsByAllIds(deliveryOrderDetail.ItemId, deliveryOrderDetail.Id, "DeliveryOrderDetail");
            foreach (var sm in smlist)
            {
                _stockMutationService.ReverseStockMutateObject(sm, _itemService);
                _stockMutationService.SoftDeleteObject(sm);
            }
            deliveryOrderDetail.IsConfirmed = false;
            deliveryOrderDetail.ConfirmationDate = null;
            deliveryOrderDetail.UpdatedAt = DateTime.Now;
            Update(deliveryOrderDetail);
            return deliveryOrderDetail;
        }

        public bool DeleteObject(int Id)
        {
            DeliveryOrderDetail prd = Find(x => x.Id == Id);
            return (Delete(prd) == 1) ? true : false;
        }

        public string SetObjectCode(DeliveryOrderDetail obj, string ParentCode)
        {
            // Code = #{PurchaseOrder.Code}/#{totalnumber + 1}
            int totalobject = FindAll(x => x.CreatedAt.Year == DateTime.Now.Year).Count() + 1;
            string Code = ParentCode + "/" + totalobject;
            return Code;
        }
    }
}
