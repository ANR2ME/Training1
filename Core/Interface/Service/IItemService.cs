using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DomainModel;
using Core.Interface.Validation;

namespace Core.Interface.Service
{
    public interface IItemService
    {
        IItemValidator GetValidator();
        IList<Item> GetAll();
        Item GetObjectById(int Id);
        Item GetObjectBySku(string Sku);
        Item CreateObject(Item item);
        Item UpdateObject(Item item);
        Item SoftDeleteObject(Item item, IStockMutationService _stockMutationService);
        bool DeleteObject(int Id);
        bool IsSkuDuplicated(Item item);
    }
}
