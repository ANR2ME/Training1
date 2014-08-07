using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DomainModel;
using Core.Interface.Repository;
using Data.Context;
using Data.Repository;
using System.Data;
using Core.Interface.Service;

namespace Data.Repository
{
    public class StockMutationRepository : EfRepository<StockMutation>, IStockMutationRepository
    {

        private StockControlEntities entities;
        public StockMutationRepository()
        {
            entities = new StockControlEntities();
        }

        public IList<StockMutation> GetAll()
        {
            return FindAll().ToList();
        }

        public IList<StockMutation> GetObjectsByItemId(int ItemId)
        {
            return FindAll(x => x.ItemId == ItemId && !x.IsDeleted).ToList();
        }

        public IList<StockMutation> GetObjectsBySourceDocumentId(int SourceDocumentId, string SourceDocumentType)
        {
            return FindAll(x => x.SourceDocumentId == SourceDocumentId && x.SourceDocumentType == SourceDocumentType && !x.IsDeleted).ToList();
        }

        public IList<StockMutation> GetObjectsBySourceDocumentDetailId(int SourceDocumentDetailId, string SourceDocumentDetailType)
        {
            return FindAll(x => x.SourceDocumentDetailId == SourceDocumentDetailId && x.SourceDocumentDetailType == SourceDocumentDetailType && !x.IsDeleted).ToList();
        }

        public IList<StockMutation> GetObjectsByAllIds(int ItemId, int SourceDocumentDetailId, string SourceDocumentDetailType)
        {
            return FindAll(x => x.ItemId == ItemId && x.SourceDocumentDetailId == SourceDocumentDetailId && x.SourceDocumentDetailType == SourceDocumentDetailType && !x.IsDeleted).ToList();
        }

        public StockMutation GetObjectById(int Id)
        {
            StockMutation sm = Find(x => x.Id == Id && !x.IsDeleted);
            if (sm != null) { sm.Errors = new Dictionary<string, string>(); }
            return sm;
        }

        public StockMutation CreateObject(StockMutation stockMutation)
        {
            stockMutation.IsDeleted = false;
            stockMutation.CreatedAt = DateTime.Now;
            return Create(stockMutation);
        }

        public StockMutation StockMutateObject(StockMutation stockMutation, IItemService _itemService)
        {
            Item item = _itemService.GetObjectById(stockMutation.ItemId);
            switch (stockMutation.ItemCase)
            {
                case "Ready" : if (stockMutation.Status == "Addition") item.Quantity += stockMutation.Quantity;
                    else if (stockMutation.Status == "Deduction") item.Quantity -= stockMutation.Quantity;
                    break;
                case "PendingReceival": if (stockMutation.Status == "Addition") item.PendingReceival += stockMutation.Quantity;
                    else if (stockMutation.Status == "Deduction") item.PendingReceival -= stockMutation.Quantity;
                    break;
                case "PendingDelivery": if (stockMutation.Status == "Addition") item.PendingDelivery += stockMutation.Quantity;
                    else if (stockMutation.Status == "Deduction") item.PendingDelivery -= stockMutation.Quantity;
                    break;
            }
            _itemService.UpdateObject(item);
            return stockMutation;
        }

        public StockMutation ReverseStockMutateObject(StockMutation stockMutation, IItemService _itemService)
        {
            Item item = _itemService.GetObjectById(stockMutation.ItemId);
            switch (stockMutation.ItemCase)
            {
                case "Ready": if (stockMutation.Status == "Addition") item.Quantity -= stockMutation.Quantity;
                    else if (stockMutation.Status == "Deduction") item.Quantity += stockMutation.Quantity;
                    break;
                case "PendingReceival": if (stockMutation.Status == "Addition") item.PendingReceival -= stockMutation.Quantity;
                    else if (stockMutation.Status == "Deduction") item.PendingReceival += stockMutation.Quantity;
                    break;
                case "PendingDelivery": if (stockMutation.Status == "Addition") item.PendingDelivery -= stockMutation.Quantity;
                    else if (stockMutation.Status == "Deduction") item.PendingDelivery += stockMutation.Quantity;
                    break;
            }
            _itemService.UpdateObject(item);
            return stockMutation;
        }

        public StockMutation SoftDeleteObject(StockMutation stockMutation)
        {
            stockMutation.IsDeleted = true;
            stockMutation.DeletedAt = DateTime.Now;
            Update(stockMutation);
            return stockMutation;
        }

        public bool DeleteObject(int Id)
        {
            StockMutation sm = Find(x => x.Id == Id);
            return (Delete(sm) == 1) ? true : false;
        }
    }
}
