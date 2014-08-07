using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Interface.Service;

namespace Core.Interface.Repository
{
    public interface IStockMutationRepository : IRepository<StockMutation>
    {
        IList<StockMutation> GetAll();
        IList<StockMutation> GetObjectsByItemId(int ItemId);
        IList<StockMutation> GetObjectsBySourceDocumentId(int SourceDocumentId, string SourceDocumentType);
        IList<StockMutation> GetObjectsBySourceDocumentDetailId(int SourceDocumentDetailId, string SourceDocumentDetailType);
        IList<StockMutation> GetObjectsByAllIds(int ItemId, int SourceDocumentDetailId, string SourceDocumentDetailType);
        StockMutation GetObjectById(int Id);
        StockMutation CreateObject(StockMutation stockMutation);
        StockMutation StockMutateObject(StockMutation stockMutation, IItemService _itemService);
        StockMutation ReverseStockMutateObject(StockMutation stockMutation, IItemService _itemService);
        StockMutation SoftDeleteObject(StockMutation stockMutation);
        bool DeleteObject(int Id);
    }
}
