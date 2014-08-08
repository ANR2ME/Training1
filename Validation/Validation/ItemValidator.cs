using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using Core.DomainModel;
using Core.Interface.Validation;
using Core.Interface.Service;

namespace Validation.Validation
{
    public class ItemValidator : IItemValidator
    {
        public Item VHasSku(Item item)
        {
            if (item.Sku == "")
            {
                item.Errors.Add("SKU", "Tidak boleh kosong");
            }
            return item;
        }

        public Item VHasUniqueSku(Item item, IItemService _itemService)
        {
            if (_itemService.IsSkuDuplicated(item))
            {
                item.Errors.Add("SKU", "Tidak boleh diduplikasi");
            }
            return item;
        }

        public Item VHasDescription(Item item)
        {
            if (item.Description == "")
            {
                item.Errors.Add("Description", "Tidak boleh kosong");
            }
            return item;
        }

        public Item VHasStockMutation(Item item, IStockMutationService _stockMutationService)
        {
            IList<StockMutation> stockMutations = _stockMutationService.GetObjectsByItemId(item.Id);
            if (stockMutations.Any())
            {
                item.Errors.Add("StockMutations", "Tidak boleh terasosiasi");
            }
            return item;
        }

        public Item VCreateObject(Item item, IItemService _itemService)
        {
            VHasSku(item);
            VHasUniqueSku(item, _itemService);
            VHasDescription(item);
            return item;
        }

        public Item VUpdateObject(Item item, IItemService _itemService)
        {
            VHasSku(item);
            VHasUniqueSku(item, _itemService);
            VHasDescription(item);
            return item;
        }

        public Item VDeleteObject(Item item, IStockMutationService _stockMutationService)
        {
            VHasStockMutation(item, _stockMutationService);
            return item;
        }

        public bool ValidCreateObject(Item item, IItemService _itemService)
        {
            VCreateObject(item, _itemService);
            return isValid(item);
        }

        public bool ValidUpdateObject(Item item, IItemService _itemService)
        {
            item.Errors.Clear();
            VUpdateObject(item, _itemService);
            return isValid(item);
        }

        public bool ValidDeleteObject(Item item, IStockMutationService _stockMutationService)
        {
            item.Errors.Clear();
            VDeleteObject(item, _stockMutationService);
            return isValid(item);
        }

        public bool isValid(Item obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }

        public string PrintError(Item obj)
        {
            string erroroutput = "";
            KeyValuePair<string, string> first = obj.Errors.ElementAt(0);
            erroroutput += first.Key + "," + first.Value;
            foreach (KeyValuePair<string, string> pair in obj.Errors.Skip(1))
            {
                erroroutput += Environment.NewLine;
                erroroutput += pair.Key + "," + pair.Value;
            }
            return erroroutput;
        }

    }
}
