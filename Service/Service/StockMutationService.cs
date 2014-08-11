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
    public class StockMutationService : IStockMutationService
    {
        private IStockMutationRepository _repository;
        private IStockMutationValidator _validator;
        public StockMutationService(IStockMutationRepository _stockMutationRepository, IStockMutationValidator _stockMutationValidator)
        {
            _repository = _stockMutationRepository;
            _validator = _stockMutationValidator;
        }

        public IStockMutationValidator GetValidator()
        {
            return _validator;
        }

        public IList<StockMutation> GetAll()
        {
            return _repository.GetAll();
        }

        public IList<StockMutation> GetObjectsByItemId(int ItemId)
        {
            return _repository.GetObjectsByItemId(ItemId);
        }

        public IList<StockMutation> GetObjectsByAllIds(int ItemId, int SourceDocumentDetailId, string SourceDocumentType)
        {
            return _repository.GetObjectsByAllIds(ItemId, SourceDocumentDetailId, SourceDocumentType);
        }

        public StockMutation GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public StockMutation CreateObject(StockMutation stockMutation, IItemService _itemService)
        {
            stockMutation.Errors = new Dictionary<String, String>();
            if (_validator.ValidCreateObject(stockMutation, _itemService))
            {
                return _repository.CreateObject(stockMutation);
            }
            else
            {
                return stockMutation;
            }
        }

        public StockMutation CreateObject(int ItemId, IItemService _itemService)
        {
            StockMutation stockMutation = new StockMutation()
            {
                ItemId = ItemId,
                
            };
            return this.CreateObject(stockMutation, _itemService);
        }

        public StockMutation StockMutateObject(StockMutation stockMutation, IItemService _itemService)
        {
            return (stockMutation = _validator.ValidStockMutateObject(stockMutation) ? _repository.StockMutateObject(stockMutation, _itemService) : stockMutation);
        }

        public StockMutation ReverseStockMutateObject(StockMutation stockMutation, IItemService _itemService)
        {
            return (stockMutation = _validator.ValidReverseStockMutateObject(stockMutation) ? _repository.ReverseStockMutateObject(stockMutation, _itemService) : stockMutation);
        }

        public StockMutation SoftDeleteObject(StockMutation stockMutation)
        {
            return (stockMutation = _validator.ValidDeleteObject(stockMutation) ? _repository.SoftDeleteObject(stockMutation) : stockMutation);
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }
    }
}
