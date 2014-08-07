using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DomainModel;
using Core.Interface.Repository;
using Core.Interface.Service;
using Core.Interface.Validation;
using Validation.Validation;

namespace Service.Service
{
    public class StockAdjustmentService : IStockAdjustmentService
    {
        private IStockAdjustmentRepository _repository;
        private IStockAdjustmentValidator _validator;
        public StockAdjustmentService(IStockAdjustmentRepository _stockAdjustmentRepository, IStockAdjustmentValidator _stockAdjustmentValidator)
        {
            _repository = _stockAdjustmentRepository;
            _validator = _stockAdjustmentValidator;
        }

        public IStockAdjustmentValidator GetValidator()
        {
            return _validator;
        }

        public IList<StockAdjustment> GetAll()
        {
            return _repository.GetAll();
        }

        public StockAdjustment GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public StockAdjustment CreateObject(StockAdjustment stockAdjustment)
        {
            stockAdjustment.Errors = new Dictionary<String, String>();
            return (stockAdjustment = _validator.ValidCreateObject(stockAdjustment) ? _repository.CreateObject(stockAdjustment) : stockAdjustment);
        }

        public StockAdjustment UpdateObject(StockAdjustment stockAdjustment)
        {
            return (stockAdjustment = _validator.ValidUpdateObject(stockAdjustment) ? _repository.UpdateObject(stockAdjustment) : stockAdjustment);
        }

        public StockAdjustment ConfirmObject(StockAdjustment stockAdjustment, IStockAdjustmentDetailService _stockAdjustmentDetailService, IStockMutationService _stockMutationService, IItemService _itemService)
        {

            return (stockAdjustment = _validator.ValidConfirmObject(stockAdjustment, _stockAdjustmentDetailService, _itemService) ? _repository.ConfirmObject(stockAdjustment, _stockAdjustmentDetailService, _stockMutationService, _itemService) : stockAdjustment);
        }

        public StockAdjustment UnconfirmObject(StockAdjustment stockAdjustment, IStockAdjustmentDetailService _stockAdjustmentDetailService, IStockMutationService _stockMutationService, IItemService _itemService)
        {

            return (stockAdjustment = _validator.ValidUnconfirmObject(stockAdjustment, _stockAdjustmentDetailService, _itemService) ? _repository.UnconfirmObject(stockAdjustment, _stockAdjustmentDetailService, _stockMutationService, _itemService) : stockAdjustment);
        }

        public StockAdjustment SoftDeleteObject(StockAdjustment stockAdjustment, IStockAdjustmentDetailService _stockAdjustmentDetailService)
        {
            return (stockAdjustment = _validator.ValidDeleteObject(stockAdjustment, _stockAdjustmentDetailService) ? _repository.SoftDeleteObject(stockAdjustment) : stockAdjustment);
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }
    }
}
