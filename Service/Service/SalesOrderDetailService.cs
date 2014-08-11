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
    public class SalesOrderDetailService : ISalesOrderDetailService
    {
        private ISalesOrderDetailRepository _repository;
        private ISalesOrderDetailValidator _validator;
        public SalesOrderDetailService(ISalesOrderDetailRepository _salesOrderDetailRepository, ISalesOrderDetailValidator _salesOrderDetailValidator)
        {
            _repository = _salesOrderDetailRepository;
            _validator = _salesOrderDetailValidator;
        }

        public ISalesOrderDetailValidator GetValidator()
        {
            return _validator;
        }

        public IList<SalesOrderDetail> GetAll()
        {
            return _repository.GetAll();
        }

        public IList<SalesOrderDetail> GetObjectsByItemId(int ItemId)
        {
            return _repository.GetObjectsByItemId(ItemId);
        }

        public IList<SalesOrderDetail> GetObjectsBySalesOrderId(int SalesOrderId)
        {
            return _repository.GetObjectsBySalesOrderId(SalesOrderId);
        }

        public IList<SalesOrderDetail> GetConfirmedObjectsBySalesOrderId(int SalesOrderId)
        {
            return _repository.GetConfirmedObjectsBySalesOrderId(SalesOrderId);
        }

        public SalesOrderDetail GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public SalesOrderDetail CreateObject(SalesOrderDetail salesOrderDetail, ISalesOrderService _salesOrderService, IItemService _itemService)
        {
            salesOrderDetail.Errors = new Dictionary<String, String>();
            SalesOrder sa = _salesOrderService.GetObjectById(salesOrderDetail.SalesOrderId);
            return (salesOrderDetail = _validator.ValidCreateObject(salesOrderDetail, this, _salesOrderService, _itemService) ? _repository.CreateObject(salesOrderDetail, sa.Code) : salesOrderDetail);
        }

        public SalesOrderDetail UpdateObject(SalesOrderDetail salesOrderDetail, ISalesOrderService _salesOrderService, IItemService _itemService)
        {
            return (salesOrderDetail = _validator.ValidUpdateObject(salesOrderDetail, this, _salesOrderService, _itemService) ? _repository.UpdateObject(salesOrderDetail) : salesOrderDetail);
        }

        public SalesOrderDetail SoftDeleteObject(SalesOrderDetail salesOrderDetail)
        {
            return (salesOrderDetail = _validator.ValidDeleteObject(salesOrderDetail, this) ? _repository.SoftDeleteObject(salesOrderDetail) : salesOrderDetail);
        }

        public SalesOrderDetail ConfirmObject(SalesOrderDetail salesOrderDetail, IStockMutationService _stockMutationService, IItemService _itemService)
        {

            return (salesOrderDetail = _validator.ValidConfirmObject(salesOrderDetail, _itemService) ? _repository.ConfirmObject(salesOrderDetail, _stockMutationService, _itemService) : salesOrderDetail);
        }

        public SalesOrderDetail UnconfirmObject(SalesOrderDetail salesOrderDetail, IStockMutationService _stockMutationService, IItemService _itemService, IDeliveryOrderDetailService _deliveryOrderDetailService)
        {

            return (salesOrderDetail = _validator.ValidUnconfirmObject(salesOrderDetail, _itemService, _deliveryOrderDetailService) ? _repository.UnconfirmObject(salesOrderDetail, _stockMutationService, _itemService) : salesOrderDetail);
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }
    }
}
