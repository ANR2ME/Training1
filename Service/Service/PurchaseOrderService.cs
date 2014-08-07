using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DomainModel;
using Core.Interface.Service;
using Core.Interface.Validation;
using Data.Repository;

namespace Service.Service
{
    public class PurchaseOrderService : IPurchaseOrderService
    {
        private IPurchaseOrderRepository _repository;
        private IPurchaseOrderValidator _validator;
        public PurchaseOrderService(IPurchaseOrderRepository _purchaseOrderRepository, IPurchaseOrderValidator _purchaseOrderValidator)
        {
            _repository = _purchaseOrderRepository;
            _validator = _purchaseOrderValidator;
        }

        public IPurchaseOrderValidator GetValidator()
        {
            return _validator;
        }

        public IList<PurchaseOrder> GetAll()
        {
            return _repository.GetAll();
        }

        public PurchaseOrder GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public PurchaseOrder CreateObject(PurchaseOrder purchaseOrder)
        {
            purchaseOrder.Errors = new Dictionary<String, String>();
            return (purchaseOrder = _validator.ValidCreateObject(purchaseOrder) ? _repository.CreateObject(purchaseOrder) : purchaseOrder);
        }

        public PurchaseOrder UpdateObject(PurchaseOrder purchaseOrder)
        {
            return (purchaseOrder = _validator.ValidUpdateObject(purchaseOrder) ? _repository.UpdateObject(purchaseOrder) : purchaseOrder);
        }

        public PurchaseOrder ConfirmObject(PurchaseOrder purchaseOrder, IPurchaseOrderDetailService _purchaseOrderDetailService, IStockMutationService _stockMutationService, IItemService _itemService)
        {

            return (purchaseOrder = _validator.ValidConfirmObject(purchaseOrder, _purchaseOrderDetailService, _itemService) ? _repository.ConfirmObject(purchaseOrder, _purchaseOrderDetailService, _stockMutationService, _itemService) : purchaseOrder);
        }

        public PurchaseOrder UnconfirmObject(PurchaseOrder purchaseOrder, IPurchaseOrderDetailService _purchaseOrderDetailService, IStockMutationService _stockMutationService, IItemService _itemService, IPurchaseReceivalDetailService _purchaseReceivalDetailService)
        {

            return (purchaseOrder = _validator.ValidUnconfirmObject(purchaseOrder) ? _repository.UnconfirmObject(purchaseOrder, _purchaseOrderDetailService, _stockMutationService, _itemService, _purchaseReceivalDetailService) : purchaseOrder);
        }

        public PurchaseOrder SoftDeleteObject(PurchaseOrder purchaseOrder, IPurchaseOrderDetailService _purchaseOrderDetailService)
        {
            return (purchaseOrder = _validator.ValidDeleteObject(purchaseOrder, _purchaseOrderDetailService) ? _repository.SoftDeleteObject(purchaseOrder) : purchaseOrder);
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }
    }
}
