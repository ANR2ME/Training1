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
    public class SalesOrderService : ISalesOrderService
    {
        private ISalesOrderRepository _repository;
        private ISalesOrderValidator _validator;
        public SalesOrderService(ISalesOrderRepository _salesOrderRepository, ISalesOrderValidator _salesOrderValidator)
        {
            _repository = _salesOrderRepository;
            _validator = _salesOrderValidator;
        }

        public ISalesOrderValidator GetValidator()
        {
            return _validator;
        }

        public IList<SalesOrder> GetAll()
        {
            return _repository.GetAll();
        }

        public IList<SalesOrder> GetObjectsByContactId(int ContactId)
        {
            return _repository.GetObjectsByContactId(ContactId);
        }

        public SalesOrder GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public SalesOrder CreateObject(SalesOrder salesOrder)
        {
            salesOrder.Errors = new Dictionary<String, String>();
            return (salesOrder = _validator.ValidCreateObject(salesOrder) ? _repository.CreateObject(salesOrder) : salesOrder);
        }

        public SalesOrder UpdateObject(SalesOrder salesOrder)
        {
            return (salesOrder = _validator.ValidUpdateObject(salesOrder) ? _repository.UpdateObject(salesOrder) : salesOrder);
        }

        public SalesOrder ConfirmObject(SalesOrder salesOrder, ISalesOrderDetailService _salesOrderDetailService, IStockMutationService _stockMutationService, IItemService _itemService)
        {

            return (salesOrder = _validator.ValidConfirmObject(salesOrder, _salesOrderDetailService, _itemService) ? _repository.ConfirmObject(salesOrder, _salesOrderDetailService, _stockMutationService, _itemService) : salesOrder);
        }

        public SalesOrder UnconfirmObject(SalesOrder salesOrder, ISalesOrderDetailService _salesOrderDetailService, IStockMutationService _stockMutationService, IItemService _itemService, IDeliveryOrderDetailService _deliveryOrderDetailService)
        {

            return (salesOrder = _validator.ValidUnconfirmObject(salesOrder) ? _repository.UnconfirmObject(salesOrder, _salesOrderDetailService, _stockMutationService, _itemService, _deliveryOrderDetailService) : salesOrder);
        }

        public SalesOrder SoftDeleteObject(SalesOrder salesOrder, ISalesOrderDetailService _salesOrderDetailService)
        {
            return (salesOrder = _validator.ValidDeleteObject(salesOrder, _salesOrderDetailService) ? _repository.SoftDeleteObject(salesOrder) : salesOrder);
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }
    }
}
