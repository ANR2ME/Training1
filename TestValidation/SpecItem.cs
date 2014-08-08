using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DomainModel;
using Core.Interface.Service;
using Data.Context;
using Data.Repository;
using NSpec;
using Service.Service;
using Validation.Validation;

namespace TestValidation
{
    public class SpecItem : nspec
    {
        Item item;

        IItemService _itemService;
        IStockMutationService _stockMutationService;

        void before_each()
        {
            var db = new StockControlEntities();
            using (db)
            {
                db.DeleteAllTables();
                _itemService = new ItemService(new ItemRepository(), new ItemValidator());
                _stockMutationService = new StockMutationService(new StockMutationRepository(), new StockMutationValidator());

                item = new Item()
                {
                    Sku = "B001",
                    Description = "Buku Tulis",
                    Quantity = 0,
                    PendingDelivery = 0,
                    PendingReceival = 0
                };
                item = _itemService.CreateObject(item);
            }
        }

        /*
        * STEPS:
        * 1. Create valid item
        * 2. Create invalid item with no description
        * 3. Create invalid items with same SKU
        * 4a. Delete item
        * 4b. Delete item with stock mutations
        */
        void item_validation()
        {
            it["valid_item"] = () =>
            {
                item.Errors.Count().should_be(0);
            };

            it["item_with_no_description"] = () =>
            {
                Item nonameitem = new Item()
                {
                    Sku = "B001",
                    Description = "",
                    Quantity = 0,
                    PendingDelivery = 0,
                    PendingReceival = 0
                };
                nonameitem = _itemService.CreateObject(nonameitem);
                nonameitem.Errors.Count().should_not_be(0);
            };

            it["item_with_same_sku"] = () =>
            {
                Item sameskuitem = new Item()
                {
                    Sku = "B001",
                    Description = "buku tulis",
                    Quantity = 0,
                    PendingDelivery = 0,
                    PendingReceival = 0
                };
                sameskuitem = _itemService.CreateObject(sameskuitem);
                sameskuitem.Errors.Count().should_not_be(0);
            };

            it["delete_item"] = () =>
            {
                item = _itemService.SoftDeleteObject(item, _stockMutationService);
                item.Errors.Count().should_be(0);
            };

            it["delete_item_with_stockmutations"] = () =>
            {
                StockMutation sm = new StockMutation()
                {
                    ItemCase = "Ready",
                    Status = "Addition",
                    ItemId = item.Id,
                    SourceDocumentId = 1,
                    SourceDocumentType = "StockAdjustment",
                    SourceDocumentDetailId = 1,
                    SourceDocumentDetailType = "StockAdjustmentDetail"
                };
                sm = _stockMutationService.CreateObject(sm);
                sm.Errors.Count().should_not_be(0);
                item = _itemService.SoftDeleteObject(item, _stockMutationService);
                item.Errors.Count().should_be(0);
            };
        }
    }
}
