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
    public class SalesOrderRepository : EfRepository<SalesOrder>, ISalesOrderRepository
    {
        private StockControlEntities entities;
        public SalesOrderRepository()
        {
            entities = new StockControlEntities();
        }

        public IList<SalesOrder> GetAll()
        {
            return FindAll().ToList();
        }

        public SalesOrder GetObjectById(int Id)
        {
            SalesOrder po = Find(x => x.Id == Id && !x.IsDeleted);
            if (po != null) { po.Errors = new Dictionary<string, string>(); }
            return po;
        }

        public SalesOrder CreateObject(SalesOrder salesOrder)
        {
            salesOrder.CreatedAt = DateTime.Now;
            salesOrder.IsDeleted = false;
            salesOrder.IsConfirmed = false;
            salesOrder.Code = SetObjectCode(salesOrder);
            return Create(salesOrder);
        }

        public SalesOrder UpdateObject(SalesOrder salesOrder)
        {
            salesOrder.UpdatedAt = DateTime.Now;
            Update(salesOrder);
            return salesOrder;
        }

        public SalesOrder SoftDeleteObject(SalesOrder salesOrder)
        {
            salesOrder.IsDeleted = true;
            salesOrder.DeletedAt = DateTime.Now;
            Update(salesOrder);
            return salesOrder;
        }

        public SalesOrder ConfirmObject(SalesOrder salesOrder, ISalesOrderDetailService _salesOrderDetailService, IStockMutationService _stockMutationService, IItemService _itemService)
        {
            salesOrder.IsConfirmed = true;
            salesOrder.ConfirmationDate = DateTime.Now;
            Update(salesOrder);
            IList<SalesOrderDetail> salesOrderDetails = _salesOrderDetailService.GetObjectsBySalesOrderId(salesOrder.Id);
            foreach (var pod in salesOrderDetails)
            {
                _salesOrderDetailService.ConfirmObject(pod, _stockMutationService, _itemService);
            }
            return salesOrder;
        }

        public SalesOrder UnconfirmObject(SalesOrder salesOrder, ISalesOrderDetailService _salesOrderDetailService, IStockMutationService _stockMutationService, IItemService _itemService, IDeliveryOrderDetailService _deliveryOrderDetailService)
        {
            salesOrder.IsConfirmed = false;
            salesOrder.ConfirmationDate = null;
            salesOrder.UpdatedAt = DateTime.Now;
            Update(salesOrder);
            IList<SalesOrderDetail> salesOrderDetails = _salesOrderDetailService.GetObjectsBySalesOrderId(salesOrder.Id);
            foreach (var pod in salesOrderDetails)
            {
                _salesOrderDetailService.UnconfirmObject(pod, _stockMutationService, _itemService, _deliveryOrderDetailService);
            }
            return salesOrder;
        }

        public bool DeleteObject(int Id)
        {
            SalesOrder po = Find(x => x.Id == Id);
            return (Delete(po) == 1) ? true : false;
        }

        public string SetObjectCode(SalesOrder obj)
        {
            // Code = #{currentyear}/#{totalnumber + 1}
            int totalobject = FindAll(x => x.CreatedAt.Year == DateTime.Now.Year).Count() + 1;
            string Code = DateTime.Now.Year.ToString() + "/" + totalobject;
            return Code;
        }
    }
}
