using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DomainModel;
using Core.Interface.Repository;
using Core.Interface.Service;
using Core.Interface.Validation;

namespace Service.Service
{
    public class PurchaseReceivalDetailService : IPurchaseReceivalDetailService
    {
        private IPurchaseReceivalDetailRepository _repository;
        private IPurchaseReceivalDetailValidator _validator;
        public PurchaseReceivalDetailService(IPurchaseReceivalDetailRepository _purchaseReceivalDetailRepository, IPurchaseReceivalDetailValidator _purchaseReceivalDetailValidator)
        {
            _repository = _purchaseReceivalDetailRepository;
            _validator = _purchaseReceivalDetailValidator;
        }

        public IPurchaseReceivalDetailValidator GetValidator()
        {
            return _validator;
        }

        public IList<PurchaseReceivalDetail> GetAll()
        {
            return _repository.GetAll();
        }

        public IList<PurchaseReceivalDetail> GetObjectsByPurchaseReceivalId(int PurchaseReceivalId)
        {
            return _repository.GetObjectsByPurchaseReceivalId(PurchaseReceivalId);
        }

        public IList<PurchaseReceivalDetail> GetConfirmedObjectsByPurchaseReceivalId(int PurchaseReceivalId)
        {
            return _repository.GetConfirmedObjectsByPurchaseReceivalId(PurchaseReceivalId);
        }

        public IList<PurchaseReceivalDetail> GetObjectsByPurchaseOrderDetailId(int PurchaseOrderDetailId)
        {
            return _repository.GetObjectsByPurchaseOrderDetailId(PurchaseOrderDetailId);
        }

        public PurchaseReceivalDetail GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public PurchaseReceivalDetail CreateObject(PurchaseReceivalDetail purchaseReceivalDetail, IPurchaseReceivalService _purchaseReceivalService, IPurchaseOrderDetailService _purchaseOrderDetailService)
        {
            purchaseReceivalDetail.Errors = new Dictionary<String, String>();
            PurchaseReceival sa = _purchaseReceivalService.GetObjectById(purchaseReceivalDetail.PurchaseReceivalId);
            return (purchaseReceivalDetail = _validator.ValidCreateObject(purchaseReceivalDetail, this, _purchaseOrderDetailService) ? _repository.CreateObject(purchaseReceivalDetail, sa.Code) : purchaseReceivalDetail);
        }

        public PurchaseReceivalDetail UpdateObject(PurchaseReceivalDetail purchaseReceivalDetail)
        {
            return (purchaseReceivalDetail = _validator.ValidUpdateObject(purchaseReceivalDetail, this) ? _repository.UpdateObject(purchaseReceivalDetail) : purchaseReceivalDetail);
        }

        public PurchaseReceivalDetail SoftDeleteObject(PurchaseReceivalDetail purchaseReceivalDetail)
        {
            return (purchaseReceivalDetail = _validator.ValidDeleteObject(purchaseReceivalDetail, this) ? _repository.SoftDeleteObject(purchaseReceivalDetail) : purchaseReceivalDetail);
        }

        public PurchaseReceivalDetail ConfirmObject(PurchaseReceivalDetail purchaseReceivalDetail, IStockMutationService _stockMutationService, IItemService _itemService, IPurchaseOrderDetailService _purchaseOrderDetailService)
        {

            return (purchaseReceivalDetail = _validator.ValidConfirmObject(purchaseReceivalDetail, _itemService, _purchaseOrderDetailService) ? _repository.ConfirmObject(purchaseReceivalDetail, _stockMutationService, _itemService) : purchaseReceivalDetail);
        }

        public PurchaseReceivalDetail UnconfirmObject(PurchaseReceivalDetail purchaseReceivalDetail, IStockMutationService _stockMutationService, IItemService _itemService)
        {

            return (purchaseReceivalDetail = _validator.ValidUnconfirmObject(purchaseReceivalDetail, _itemService) ? _repository.UnconfirmObject(purchaseReceivalDetail, _stockMutationService, _itemService) : purchaseReceivalDetail);
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }
    }
}
