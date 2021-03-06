﻿using System;
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
        IStockAdjustmentService _stockAdjustmentService;
        IStockAdjustmentDetailService _stockAdjustmentDetailService;

        void before_each()
        {
            var db = new StockControlEntities();
            using (db)
            {
                db.DeleteAllTables();
                _itemService = new ItemService(new ItemRepository(), new ItemValidator());
                _stockMutationService = new StockMutationService(new StockMutationRepository(), new StockMutationValidator());
                _stockAdjustmentService = new StockAdjustmentService(new StockAdjustmentRepository(), new StockAdjustmentValidator());
                _stockAdjustmentDetailService = new StockAdjustmentDetailService(new StockAdjustmentDetailRepository(), new StockAdjustmentDetailValidator());

                item = new Item()
                {
                    Sku = "B001",
                    Description = "Buku Tulis AA",
                    Quantity = 0,
                    PendingDelivery = 0,
                    PendingReceival = 0
                };
                item = _itemService.CreateObject(item);
            }
        }

        /*
        * STEPS:
        * 1a. Create valid item
        * 1b. Create [invalid] item with no description
        * 1c. Create [invalid] item with no SKU
        * 1d. Create [invalid] items with same SKU
        * 2a. Update valid item
        * 2b. Update [invalid] item with no description
        * 2c. Update [invalid] item with no SKU
        * 2d. Update [invalid] item with same SKU
        * 3a. Delete item
        * 3b. Delete item with stock mutations
        */
        void item_validation()
        {
            it["create_valid_item"] = () =>
            {
                item.Errors.Count().should_be(0);
            };

            it["create_item_with_no_description"] = () =>
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

            it["create_item_with_no_sku"] = () =>
            {
                Item nonameitem = new Item()
                {
                    Sku = "",
                    Description = "Buku",
                    Quantity = 0,
                    PendingDelivery = 0,
                    PendingReceival = 0
                };
                nonameitem = _itemService.CreateObject(nonameitem);
                nonameitem.Errors.Count().should_not_be(0);
            };

            it["create_item_with_same_sku"] = () =>
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

            it["update_valid_item"] = () =>
            {
                item.Sku = "B002";
                item.Description = "Buku Tulis BB";
                _itemService.UpdateObject(item);
                item.Errors.Count().should_be(0);
            };

            it["update_item_with_no_description"] = () =>
            {
                item.Sku = "B002";
                item.Description = "";
                _itemService.UpdateObject(item);
                item.Errors.Count().should_not_be(0);
            };

            it["update_item_with_no_sku"] = () =>
            {
                item.Sku = "";
                item.Description = "Buku";
                _itemService.UpdateObject(item);
                item.Errors.Count().should_not_be(0);
            };

            it["new_create_and_update_item_with_same_sku"] = () =>
            {
                Item sameskuitem = new Item()
                {
                    Sku = "B003",
                    Description = "buku tulis",
                    Quantity = 0,
                    PendingDelivery = 0,
                    PendingReceival = 0
                };
                sameskuitem = _itemService.CreateObject(sameskuitem);
                if (sameskuitem.Errors.Count() > 0) Console.WriteLine("sameskuitem.Error:{0}", sameskuitem.Errors.FirstOrDefault());
                sameskuitem.Errors.Count().should_be(0);

                sameskuitem.Sku = "B001";
                _itemService.UpdateObject(sameskuitem);
                sameskuitem.Errors.Count().should_not_be(0);
            };

            it["delete_item"] = () =>
            {
                item = _itemService.SoftDeleteObject(item, _stockMutationService);
                item.Errors.Count().should_be(0);
            };

            it["delete_item_with_stockmutations"] = () =>
            {
                StockAdjustment sa = new StockAdjustment()
                {
                    AdjustmentDate = DateTime.Now
                };
                _stockAdjustmentService.CreateObject(sa);

                StockAdjustmentDetail sad = new StockAdjustmentDetail()
                {
                    ItemId = item.Id,
                    StockAdjustmentId = sa.Id,
                    Quantity = 100
                };
                _stockAdjustmentDetailService.CreateObject(sad, _stockAdjustmentService, _itemService);

                StockMutation sm = new StockMutation()
                {
                    ItemCase = "Ready",
                    Status = "Addition",
                    ItemId = item.Id,
                    Quantity = 10,
                    SourceDocumentId = sa.Id,
                    SourceDocumentType = "StockAdjustment",
                    SourceDocumentDetailId = sad.Id,
                    SourceDocumentDetailType = "StockAdjustmentDetail"
                };
                sm = _stockMutationService.CreateObject(sm, _itemService);
                if (sm.Errors.Count() > 0) Console.WriteLine("sm.Error:{0}", sm.Errors.FirstOrDefault());
                sm.Errors.Count().should_be(0); 

                item = _itemService.SoftDeleteObject(item, _stockMutationService);
                item.Errors.Count().should_not_be(0);
            };
        }
    }
}
