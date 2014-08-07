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
    public class PurchaseOrderDetailService : IPurchaseOrderDetailService
    {
        private IPurchaseOrderDetailRepository _repository;
        private IPurchaseOrderDetailValidator _validator;
        public PurchaseOrderDetailService(IPurchaseOrderDetailRepository _purchaseOrderDetailRepository, IPurchaseOrderDetailValidator _purchaseOrderDetailValidator)
        {
            _repository = _purchaseOrderDetailRepository;
            _validator = _purchaseOrderDetailValidator;
        }

        public IPurchaseOrderDetailValidator GetValidator()
        {
            return _validator;
        }

        public IList<PurchaseOrderDetail> GetAll()
        {
            return _repository.GetAll();
        }

        public IList<PurchaseOrderDetail> GetObjectsByPurchaseOrderId(int PurchaseOrderId)
        {
            return _repository.GetObjectsByPurchaseOrderId(PurchaseOrderId);
        }

        public IList<PurchaseOrderDetail> GetConfirmedObjectsByPurchaseOrderId(int PurchaseOrderId)
        {
            return _repository.GetConfirmedObjectsByPurchaseOrderId(PurchaseOrderId);
        }

        public PurchaseOrderDetail GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public PurchaseOrderDetail CreateObject(PurchaseOrderDetail purchaseOrderDetail, IPurchaseOrderService _purchaseOrderService)
        {
            purchaseOrderDetail.Errors = new Dictionary<String, String>();
            PurchaseOrder sa = _purchaseOrderService.GetObjectById(purchaseOrderDetail.PurchaseOrderId);
            return (purchaseOrderDetail = _validator.ValidCreateObject(purchaseOrderDetail, this) ? _repository.CreateObject(purchaseOrderDetail, sa.Code) : purchaseOrderDetail);
        }

        public PurchaseOrderDetail UpdateObject(PurchaseOrderDetail purchaseOrderDetail)
        {
            return (purchaseOrderDetail = _validator.ValidUpdateObject(purchaseOrderDetail, this) ? _repository.UpdateObject(purchaseOrderDetail) : purchaseOrderDetail);
        }

        public PurchaseOrderDetail SoftDeleteObject(PurchaseOrderDetail purchaseOrderDetail)
        {
            return (purchaseOrderDetail = _validator.ValidDeleteObject(purchaseOrderDetail, this) ? _repository.SoftDeleteObject(purchaseOrderDetail) : purchaseOrderDetail);
        }

        public PurchaseOrderDetail ConfirmObject(PurchaseOrderDetail purchaseOrderDetail, IStockMutationService _stockMutationService, IItemService _itemService)
        {

            return (purchaseOrderDetail = _validator.ValidConfirmObject(purchaseOrderDetail, _itemService) ? _repository.ConfirmObject(purchaseOrderDetail, _stockMutationService, _itemService) : purchaseOrderDetail);
        }

        public PurchaseOrderDetail UnconfirmObject(PurchaseOrderDetail purchaseOrderDetail, IStockMutationService _stockMutationService, IItemService _itemService, IPurchaseReceivalDetailService _purchaseReceivalDetailService)
        {

            return (purchaseOrderDetail = _validator.ValidUnconfirmObject(purchaseOrderDetail, _itemService, _purchaseReceivalDetailService) ? _repository.UnconfirmObject(purchaseOrderDetail, _stockMutationService, _itemService) : purchaseOrderDetail);
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }
    }
}
