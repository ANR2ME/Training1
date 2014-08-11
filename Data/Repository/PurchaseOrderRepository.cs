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
    public class PurchaseOrderRepository : EfRepository<PurchaseOrder>, IPurchaseOrderRepository
    {
        private StockControlEntities entities;
        public PurchaseOrderRepository()
        {
            entities = new StockControlEntities();
        }

        public IList<PurchaseOrder> GetAll()
        {
            return FindAll().ToList();
        }

        public IList<PurchaseOrder> GetObjectsByContactId(int ContactId)
        {
            return FindAll(x => x.ContactId == ContactId && !x.IsDeleted).ToList();
        }

        public PurchaseOrder GetObjectById(int Id)
        {
            PurchaseOrder po = Find(x => x.Id == Id && !x.IsDeleted);
            if (po != null) { po.Errors = new Dictionary<string, string>(); }
            return po;
        }

        public PurchaseOrder CreateObject(PurchaseOrder purchaseOrder)
        {
            purchaseOrder.CreatedAt = DateTime.Now;
            purchaseOrder.IsDeleted = false;
            purchaseOrder.IsConfirmed = false;
            purchaseOrder.Code = SetObjectCode(purchaseOrder);
            return Create(purchaseOrder);
        }

        public PurchaseOrder UpdateObject(PurchaseOrder purchaseOrder)
        {
            purchaseOrder.UpdatedAt = DateTime.Now;
            Update(purchaseOrder);
            return purchaseOrder;
        }

        public PurchaseOrder SoftDeleteObject(PurchaseOrder purchaseOrder)
        {
            purchaseOrder.IsDeleted = true;
            purchaseOrder.DeletedAt = DateTime.Now;
            Update(purchaseOrder);
            return purchaseOrder;
        }

        public PurchaseOrder ConfirmObject(PurchaseOrder purchaseOrder, IPurchaseOrderDetailService _purchaseOrderDetailService, IStockMutationService _stockMutationService, IItemService _itemService)
        {
            IList<PurchaseOrderDetail> purchaseOrderDetails = _purchaseOrderDetailService.GetObjectsByPurchaseOrderId(purchaseOrder.Id);
            int unconfirmed = 0;
            foreach (var pod in purchaseOrderDetails)
            {
                _purchaseOrderDetailService.ConfirmObject(pod, _stockMutationService, _itemService);
                if (!pod.IsConfirmed) unconfirmed++;
            }
            if (unconfirmed == 0)
            {
                purchaseOrder.IsConfirmed = true;
                purchaseOrder.ConfirmationDate = DateTime.Now;
                Update(purchaseOrder);
            }
            return purchaseOrder;
        }

        public PurchaseOrder UnconfirmObject(PurchaseOrder purchaseOrder, IPurchaseOrderDetailService _purchaseOrderDetailService, IStockMutationService _stockMutationService, IItemService _itemService, IPurchaseReceivalDetailService _purchaseReceivalDetailService)
        {
            purchaseOrder.IsConfirmed = false;
            purchaseOrder.ConfirmationDate = null;
            purchaseOrder.UpdatedAt = DateTime.Now;
            Update(purchaseOrder);
            IList<PurchaseOrderDetail> purchaseOrderDetails = _purchaseOrderDetailService.GetObjectsByPurchaseOrderId(purchaseOrder.Id);
            foreach (var pod in purchaseOrderDetails)
            {
                _purchaseOrderDetailService.UnconfirmObject(pod, _stockMutationService, _itemService, _purchaseReceivalDetailService);
            }
            return purchaseOrder;
        }

        public bool DeleteObject(int Id)
        {
            PurchaseOrder po = Find(x => x.Id == Id);
            return (Delete(po) == 1) ? true : false;
        }

        public string SetObjectCode(PurchaseOrder obj)
        {
            // Code = #{currentyear}/#{totalnumber + 1}
            int totalobject = FindAll(x => x.CreatedAt.Year == DateTime.Now.Year).Count() + 1;
            string Code = DateTime.Now.Year.ToString() + "/" + totalobject;
            return Code;
        }
    }
}
