using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DomainModel;
using Core.Interface.Service;

namespace Core.Interface.Validation
{
    public interface IItemValidator
    {
        Item VHasSku(Item item);
        Item VHasUniqueSku(Item item, IItemService _itemService);
        Item VHasDescription(Item item);
        Item VHasStockMutation(Item item, IStockMutationService _stockMutationService);

        Item VCreateObject(Item item, IItemService _itemService);
        Item VUpdateObject(Item item, IItemService _itemService);
        Item VDeleteObject(Item item, IStockMutationService _stockMutationService);

        bool ValidCreateObject(Item item, IItemService _itemService);
        bool ValidUpdateObject(Item item, IItemService _itemService);
        bool ValidDeleteObject(Item item, IStockMutationService _stockMutationService);
        bool isValid(Item item);
        string PrintError(Item item);
    }
}
