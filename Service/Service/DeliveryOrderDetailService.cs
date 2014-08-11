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
    public class DeliveryOrderDetailService : IDeliveryOrderDetailService
    {
        private IDeliveryOrderDetailRepository _repository;
        private IDeliveryOrderDetailValidator _validator;
        public DeliveryOrderDetailService(IDeliveryOrderDetailRepository _deliveryOrderDetailRepository, IDeliveryOrderDetailValidator _deliveryOrderDetailValidator)
        {
            _repository = _deliveryOrderDetailRepository;
            _validator = _deliveryOrderDetailValidator;
        }

        public IDeliveryOrderDetailValidator GetValidator()
        {
            return _validator;
        }

        public IList<DeliveryOrderDetail> GetAll()
        {
            return _repository.GetAll();
        }

        public IList<DeliveryOrderDetail> GetObjectsByItemId(int ItemId)
        {
            return _repository.GetObjectsByItemId(ItemId);
        }

        public IList<DeliveryOrderDetail> GetObjectsByDeliveryOrderId(int DeliveryOrderId)
        {
            return _repository.GetObjectsByDeliveryOrderId(DeliveryOrderId);
        }

        public IList<DeliveryOrderDetail> GetConfirmedObjectsByDeliveryOrderId(int DeliveryOrderId)
        {
            return _repository.GetConfirmedObjectsByDeliveryOrderId(DeliveryOrderId);
        }

        public IList<DeliveryOrderDetail> GetObjectsBySalesOrderDetailId(int SalesOrderDetailId)
        {
            return _repository.GetObjectsBySalesOrderDetailId(SalesOrderDetailId);
        }

        public DeliveryOrderDetail GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public DeliveryOrderDetail CreateObject(DeliveryOrderDetail deliveryOrderDetail, IItemService _itemService, IDeliveryOrderService _deliveryOrderService, ISalesOrderDetailService _salesOrderDetailService)
        {
            deliveryOrderDetail.Errors = new Dictionary<String, String>();
            DeliveryOrder sa = _deliveryOrderService.GetObjectById(deliveryOrderDetail.DeliveryOrderId);
            return (deliveryOrderDetail = _validator.ValidCreateObject(deliveryOrderDetail, this, _itemService, _deliveryOrderService, _salesOrderDetailService) ? _repository.CreateObject(deliveryOrderDetail, sa.Code) : deliveryOrderDetail);
        }

        public DeliveryOrderDetail UpdateObject(DeliveryOrderDetail deliveryOrderDetail, IItemService _itemService, IDeliveryOrderService _deliveryOrderService, ISalesOrderDetailService _salesOrderDetailService)
        {
            return (deliveryOrderDetail = _validator.ValidUpdateObject(deliveryOrderDetail, this, _itemService, _deliveryOrderService, _salesOrderDetailService) ? _repository.UpdateObject(deliveryOrderDetail) : deliveryOrderDetail);
        }

        public DeliveryOrderDetail SoftDeleteObject(DeliveryOrderDetail deliveryOrderDetail)
        {
            return (deliveryOrderDetail = _validator.ValidDeleteObject(deliveryOrderDetail, this) ? _repository.SoftDeleteObject(deliveryOrderDetail) : deliveryOrderDetail);
        }

        public DeliveryOrderDetail ConfirmObject(DeliveryOrderDetail deliveryOrderDetail, IStockMutationService _stockMutationService, IItemService _itemService, ISalesOrderDetailService _salesOrderDetailService)
        {

            return (deliveryOrderDetail = _validator.ValidConfirmObject(deliveryOrderDetail, _itemService, _salesOrderDetailService) ? _repository.ConfirmObject(deliveryOrderDetail, _stockMutationService, _itemService) : deliveryOrderDetail);
        }

        public DeliveryOrderDetail UnconfirmObject(DeliveryOrderDetail deliveryOrderDetail, IStockMutationService _stockMutationService, IItemService _itemService)
        {

            return (deliveryOrderDetail = _validator.ValidUnconfirmObject(deliveryOrderDetail, _itemService) ? _repository.UnconfirmObject(deliveryOrderDetail, _stockMutationService, _itemService) : deliveryOrderDetail);
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }
    }
}
