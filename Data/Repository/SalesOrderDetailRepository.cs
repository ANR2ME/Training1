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
    public class SalesOrderDetailRepository : EfRepository<SalesOrderDetail>, ISalesOrderDetailRepository
    {
        private StockControlEntities entities;
        public SalesOrderDetailRepository()
        {
            entities = new StockControlEntities();
        }

        public IList<SalesOrderDetail> GetAll()
        {
            return FindAll().ToList();
        }

        public IList<SalesOrderDetail> GetObjectsByItemId(int ItemId)
        {
            return FindAll(x => x.ItemId == ItemId && !x.IsDeleted).ToList();
        }

        public IList<SalesOrderDetail> GetObjectsBySalesOrderId(int SalesOrderId)
        {
            return FindAll(x => x.SalesOrderId == SalesOrderId && !x.IsDeleted).ToList();
        }

        public IList<SalesOrderDetail> GetConfirmedObjectsBySalesOrderId(int SalesOrderId)
        {
            return FindAll(x => x.SalesOrderId == SalesOrderId && x.IsConfirmed && !x.IsDeleted).ToList();
        }

        public SalesOrderDetail GetObjectById(int Id)
        {
            SalesOrderDetail pod = Find(x => x.Id == Id && !x.IsDeleted);
            if (pod != null) { pod.Errors = new Dictionary<string, string>(); }
            return pod;
        }

        public SalesOrderDetail CreateObject(SalesOrderDetail salesOrderDetail, string ParentCode)
        {
            salesOrderDetail.CreatedAt = DateTime.Now;
            salesOrderDetail.IsDeleted = false;
            salesOrderDetail.IsConfirmed = false;
            salesOrderDetail.Code = SetObjectCode(salesOrderDetail, ParentCode);
            return Create(salesOrderDetail);
        }

        public SalesOrderDetail UpdateObject(SalesOrderDetail salesOrderDetail)
        {
            salesOrderDetail.UpdatedAt = DateTime.Now;
            Update(salesOrderDetail);
            return salesOrderDetail;
        }

        public SalesOrderDetail SoftDeleteObject(SalesOrderDetail salesOrderDetail)
        {
            salesOrderDetail.IsDeleted = true;
            salesOrderDetail.DeletedAt = DateTime.Now;
            Update(salesOrderDetail);
            return salesOrderDetail;
        }

        public SalesOrderDetail ConfirmObject(SalesOrderDetail salesOrderDetail, IStockMutationService _stockMutationService, IItemService _itemService)
        {
            StockMutation sm = new StockMutation()
            {
                ItemId = salesOrderDetail.ItemId,
                Status = "Addition",
                ItemCase = "PendingDelivery",
                Quantity = salesOrderDetail.Quantity,
                SourceDocumentType = "SalesOrder",
                SourceDocumentId = salesOrderDetail.SalesOrderId,
                SourceDocumentDetailType = "SalesOrderDetail",
                SourceDocumentDetailId = salesOrderDetail.Id,
            };
            _stockMutationService.CreateObject(sm, _itemService);
            _stockMutationService.StockMutateObject(sm, _itemService);
            salesOrderDetail.IsConfirmed = true;
            salesOrderDetail.ConfirmationDate = DateTime.Now;
            Update(salesOrderDetail);
            return salesOrderDetail;
        }

        public SalesOrderDetail UnconfirmObject(SalesOrderDetail salesOrderDetail, IStockMutationService _stockMutationService, IItemService _itemService)
        {
            IList<StockMutation> smlist = _stockMutationService.GetObjectsByAllIds(salesOrderDetail.ItemId, salesOrderDetail.Id, "SalesOrderDetail");
            foreach (var sm in smlist)
            {
                _stockMutationService.ReverseStockMutateObject(sm, _itemService);
                _stockMutationService.SoftDeleteObject(sm);
            }
            salesOrderDetail.IsConfirmed = false;
            salesOrderDetail.ConfirmationDate = null;
            salesOrderDetail.UpdatedAt = DateTime.Now;
            Update(salesOrderDetail);
            return salesOrderDetail;
        }

        public bool DeleteObject(int Id)
        {
            SalesOrderDetail pod = Find(x => x.Id == Id);
            return (Delete(pod) == 1) ? true : false;
        }

        public string SetObjectCode(SalesOrderDetail obj, string ParentCode)
        {
            // Code = #{SalesOrder.Code}/#{totalnumber + 1}
            int totalobject = FindAll(x => x.CreatedAt.Year == DateTime.Now.Year).Count() + 1;
            string Code = ParentCode + "/" + totalobject;
            return Code;
        }
    }
}
