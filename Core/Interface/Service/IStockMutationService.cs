using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DomainModel;
using Core.Interface.Validation;

namespace Core.Interface.Service
{
    public interface IStockMutationService
    {
        IStockMutationValidator GetValidator();
        IList<StockMutation> GetAll();
        IList<StockMutation> GetObjectsByItemId(int ItemId);
        IList<StockMutation> GetObjectsByAllIds(int ItemId, int SourceDocumentDetailId, string SourceDocumentType);
        StockMutation GetObjectById(int Id);
        StockMutation CreateObject(StockMutation stockMutation);
        StockMutation StockMutateObject(StockMutation stockMutation, IItemService _itemService);
        StockMutation ReverseStockMutateObject(StockMutation stockMutation, IItemService _itemService);
        StockMutation SoftDeleteObject(StockMutation stockMutation);
        bool DeleteObject(int Id);
    }
}
