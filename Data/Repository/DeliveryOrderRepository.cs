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
    public class DeliveryOrderRepository : EfRepository<DeliveryOrder>, IDeliveryOrderRepository
    {
        private StockControlEntities entities;
        public DeliveryOrderRepository()
        {
            entities = new StockControlEntities();
        }

        public IList<DeliveryOrder> GetAll()
        {
            return FindAll().ToList();
        }

        public DeliveryOrder GetObjectById(int Id)
        {
            DeliveryOrder pr = Find(x => x.Id == Id && !x.IsDeleted);
            if (pr != null) { pr.Errors = new Dictionary<string, string>(); }
            return pr;
        }

        public DeliveryOrder CreateObject(DeliveryOrder deliveryOrder)
        {
            deliveryOrder.CreatedAt = DateTime.Now;
            deliveryOrder.IsDeleted = false;
            deliveryOrder.IsConfirmed = false;
            deliveryOrder.Code = SetObjectCode(deliveryOrder);
            return Create(deliveryOrder);
        }

        public DeliveryOrder UpdateObject(DeliveryOrder deliveryOrder)
        {
            deliveryOrder.UpdatedAt = DateTime.Now;
            Update(deliveryOrder);
            return deliveryOrder;
        }

        public DeliveryOrder SoftDeleteObject(DeliveryOrder deliveryOrder)
        {
            deliveryOrder.IsDeleted = true;
            deliveryOrder.DeletedAt = DateTime.Now;
            Update(deliveryOrder);
            return deliveryOrder;
        }

        public DeliveryOrder ConfirmObject(DeliveryOrder deliveryOrder, IDeliveryOrderDetailService _deliveryOrderDetailService, IStockMutationService _stockMutationService, IItemService _itemService, ISalesOrderDetailService _salesOrderDetailService)
        {
            IList<DeliveryOrderDetail> deliveryOrderDetails = _deliveryOrderDetailService.GetObjectsByDeliveryOrderId(deliveryOrder.Id);
            int unconfirmed = 0;
            foreach (var prd in deliveryOrderDetails)
            {
                _deliveryOrderDetailService.ConfirmObject(prd, _stockMutationService, _itemService, _salesOrderDetailService);
                if (!prd.IsConfirmed) unconfirmed++;
            }
            if (unconfirmed == 0)
            {
                deliveryOrder.IsConfirmed = true;
                deliveryOrder.ConfirmationDate = DateTime.Now;
                Update(deliveryOrder);
            }
            return deliveryOrder;
        }

        public DeliveryOrder UnconfirmObject(DeliveryOrder deliveryOrder, IDeliveryOrderDetailService _deliveryOrderDetailService, IStockMutationService _stockMutationService, IItemService _itemService)
        {
            deliveryOrder.IsConfirmed = false;
            deliveryOrder.ConfirmationDate = null;
            deliveryOrder.UpdatedAt = DateTime.Now;
            Update(deliveryOrder);
            IList<DeliveryOrderDetail> deliveryOrderDetails = _deliveryOrderDetailService.GetObjectsByDeliveryOrderId(deliveryOrder.Id);
            foreach (var prd in deliveryOrderDetails)
            {
                _deliveryOrderDetailService.UnconfirmObject(prd, _stockMutationService, _itemService);
            }
            return deliveryOrder;
        }

        public bool DeleteObject(int Id)
        {
            DeliveryOrder pr = Find(x => x.Id == Id);
            return (Delete(pr) == 1) ? true : false;
        }

        public string SetObjectCode(DeliveryOrder obj)
        {
            // Code = #{currentyear}/#{totalnumber + 1}
            int totalobject = FindAll(x => x.CreatedAt.Year == DateTime.Now.Year).Count() + 1;
            string Code = DateTime.Now.Year.ToString() + "/" + totalobject;
            return Code;
        }
    }
}
