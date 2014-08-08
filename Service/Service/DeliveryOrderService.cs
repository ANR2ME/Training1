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
    public class DeliveryOrderService : IDeliveryOrderService
    {
        private IDeliveryOrderRepository _repository;
        private IDeliveryOrderValidator _validator;
        public DeliveryOrderService(IDeliveryOrderRepository _deliveryOrderRepository, IDeliveryOrderValidator _deliveryOrderValidator)
        {
            _repository = _deliveryOrderRepository;
            _validator = _deliveryOrderValidator;
        }

        public IDeliveryOrderValidator GetValidator()
        {
            return _validator;
        }

        public IList<DeliveryOrder> GetAll()
        {
            return _repository.GetAll();
        }

        public DeliveryOrder GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public DeliveryOrder CreateObject(DeliveryOrder deliveryOrder, ISalesOrderService _salesOrderService)
        {
            deliveryOrder.Errors = new Dictionary<String, String>();
            return (deliveryOrder = _validator.ValidCreateObject(deliveryOrder, _salesOrderService) ? _repository.CreateObject(deliveryOrder) : deliveryOrder);
        }

        public DeliveryOrder UpdateObject(DeliveryOrder deliveryOrder, ISalesOrderService _salesOrderService)
        {
            return (deliveryOrder = _validator.ValidUpdateObject(deliveryOrder, _salesOrderService) ? _repository.UpdateObject(deliveryOrder) : deliveryOrder);
        }

        public DeliveryOrder ConfirmObject(DeliveryOrder deliveryOrder, IDeliveryOrderDetailService _deliveryOrderDetailService, IStockMutationService _stockMutationService, IItemService _itemService, ISalesOrderDetailService _salesOrderDetailService)
        {

            return (deliveryOrder = _validator.ValidConfirmObject(deliveryOrder, _deliveryOrderDetailService, _itemService) ? _repository.ConfirmObject(deliveryOrder, _deliveryOrderDetailService, _stockMutationService, _itemService, _salesOrderDetailService) : deliveryOrder);
        }

        public DeliveryOrder UnconfirmObject(DeliveryOrder deliveryOrder, IDeliveryOrderDetailService _deliveryOrderDetailService, IStockMutationService _stockMutationService, IItemService _itemService)
        {

            return (deliveryOrder = _validator.ValidUnconfirmObject(deliveryOrder) ? _repository.UnconfirmObject(deliveryOrder, _deliveryOrderDetailService, _stockMutationService, _itemService) : deliveryOrder);
        }

        public DeliveryOrder SoftDeleteObject(DeliveryOrder deliveryOrder, IDeliveryOrderDetailService _deliveryOrderDetailService)
        {
            return (deliveryOrder = _validator.ValidDeleteObject(deliveryOrder, _deliveryOrderDetailService) ? _repository.SoftDeleteObject(deliveryOrder) : deliveryOrder);
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }
    }
}
