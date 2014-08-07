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
    public class StockAdjustmentDetailService : IStockAdjustmentDetailService
    {
        private IStockAdjustmentDetailRepository _repository;
        private IStockAdjustmentDetailValidator _validator;
        public StockAdjustmentDetailService(IStockAdjustmentDetailRepository _stockAdjustmentDetailRepository, IStockAdjustmentDetailValidator _stockAdjustmentDetailValidator)
        {
            _repository = _stockAdjustmentDetailRepository;
            _validator = _stockAdjustmentDetailValidator;
        }

        public IStockAdjustmentDetailValidator GetValidator()
        {
            return _validator;
        }

        public IList<StockAdjustmentDetail> GetAll()
        {
            return _repository.GetAll();
        }

        public IList<StockAdjustmentDetail> GetObjectsByStockAdjustmentId(int StockAdjustmentId)
        {
            return _repository.GetObjectsByStockAdjustmentId(StockAdjustmentId);
        }

        public IList<StockAdjustmentDetail> GetConfirmedObjectsByStockAdjustmentId(int StockAdjustmentId)
        {
            return _repository.GetConfirmedObjectsByStockAdjustmentId(StockAdjustmentId);
        }

        public StockAdjustmentDetail GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public StockAdjustmentDetail CreateObject(StockAdjustmentDetail stockAdjustmentDetail, IStockAdjustmentService _stockAdjustmentService)
        {
            stockAdjustmentDetail.Errors = new Dictionary<String, String>();
            StockAdjustment sa = _stockAdjustmentService.GetObjectById(stockAdjustmentDetail.StockAdjustmentId);
            return (stockAdjustmentDetail = _validator.ValidCreateObject(stockAdjustmentDetail, this) ? _repository.CreateObject(stockAdjustmentDetail, sa.Code) : stockAdjustmentDetail);
        }

        public StockAdjustmentDetail UpdateObject(StockAdjustmentDetail stockAdjustmentDetail)
        {
            return (stockAdjustmentDetail = _validator.ValidUpdateObject(stockAdjustmentDetail, this) ? _repository.UpdateObject(stockAdjustmentDetail) : stockAdjustmentDetail);
        }

        public StockAdjustmentDetail SoftDeleteObject(StockAdjustmentDetail stockAdjustmentDetail)
        {
            return (stockAdjustmentDetail = _validator.ValidDeleteObject(stockAdjustmentDetail, this) ? _repository.SoftDeleteObject(stockAdjustmentDetail) : stockAdjustmentDetail);
        }

        public StockAdjustmentDetail ConfirmObject(StockAdjustmentDetail stockAdjustmentDetail, IStockMutationService _stockMutationService, IItemService _itemService)
        {

            return (stockAdjustmentDetail = _validator.ValidConfirmObject(stockAdjustmentDetail, _itemService) ? _repository.ConfirmObject(stockAdjustmentDetail, _stockMutationService, _itemService) : stockAdjustmentDetail);
        }

        public StockAdjustmentDetail UnconfirmObject(StockAdjustmentDetail stockAdjustmentDetail, IStockMutationService _stockMutationService, IItemService _itemService)
        {

            return (stockAdjustmentDetail = _validator.ValidUnconfirmObject(stockAdjustmentDetail, _itemService) ? _repository.UnconfirmObject(stockAdjustmentDetail, _stockMutationService, _itemService) : stockAdjustmentDetail);
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }
    }
}
