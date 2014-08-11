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
    public class SpecSalesOrder : nspec
    {
        int jumlah1, jumlah2;
        DateTime now;
        Contact contact, contact2;
        Item item;
        SalesOrder valid_induk1, valid_induk2, invalid_induk1,invalid_induk2;
        SalesOrderDetail valid_anak1, valid_anak2, invalid_anak1, invalid_anak2, invalid_anak3;

        IItemService _itemService;
        IStockMutationService _stockMutationService;
        IContactService _contactService;
        ISalesOrderDetailService _salesOrderDetailService;
        ISalesOrderService _salesOrderService;
        IDeliveryOrderService _deliveryOrderService;
        IDeliveryOrderDetailService _deliveryOrderDetailService;

        void before_each()
        {
            var db = new StockControlEntities();
            using (db)
            {
                db.DeleteAllTables();
                _itemService = new ItemService(new ItemRepository(), new ItemValidator());
                _contactService = new ContactService(new ContactRepository(), new ContactValidator());
                _stockMutationService = new StockMutationService(new StockMutationRepository(), new StockMutationValidator());
                _salesOrderDetailService = new SalesOrderDetailService(new SalesOrderDetailRepository(), new SalesOrderDetailValidator());
                _salesOrderService = new SalesOrderService(new SalesOrderRepository(), new SalesOrderValidator());
                _deliveryOrderService = new DeliveryOrderService(new DeliveryOrderRepository(), new DeliveryOrderValidator());
                _deliveryOrderDetailService = new DeliveryOrderDetailService(new DeliveryOrderDetailRepository(), new DeliveryOrderDetailValidator());

                now = DateTime.Now;
                jumlah1 = 30;
                jumlah2 = 40;

                contact = new Contact()
                {
                    Address = "Jl. XXX",
                    PhoneNumber = "021-3456",
                    Name = "Adam"
                };
                contact = _contactService.CreateObject(contact);
                if (contact.Errors.Count() > 0) Console.WriteLine("contact.Error:{0}", contact.Errors.FirstOrDefault());
                contact2 = new Contact()
                {
                    Address = "Jl. YYY",
                    PhoneNumber = "021-7890",
                    Name = "Bernard"
                };
                contact2 = _contactService.CreateObject(contact2);
                if (contact2.Errors.Count() > 0) Console.WriteLine("contact2.Error:{0}", contact2.Errors.FirstOrDefault());

                item = new Item()
                {
                    Sku = "B001",
                    Description = "Buku Tulis AA",
                    Quantity = jumlah1,
                    PendingDelivery = 0,
                    PendingReceival = 0
                };
                _itemService.CreateObject(item);
                if (item.Errors.Count() > 0) Console.WriteLine("item.Error:{0}", item.Errors.FirstOrDefault());

                valid_induk1 = new SalesOrder()
                {
                    ContactId = contact.Id,
                    SalesDate = now
                };
                _salesOrderService.CreateObject(valid_induk1, _contactService);

                valid_induk2 = new SalesOrder()
                {
                    ContactId = contact.Id,
                    SalesDate = now
                };
                _salesOrderService.CreateObject(valid_induk2, _contactService);

                invalid_induk1 = new SalesOrder()
                {
                    SalesDate = now
                };
                _salesOrderService.CreateObject(invalid_induk1, _contactService);

                invalid_induk2 = new SalesOrder()
                {
                    ContactId = contact.Id
                };
                _salesOrderService.CreateObject(invalid_induk2, _contactService);

                valid_anak1 = new SalesOrderDetail()
                {
                    ItemId = item.Id,
                    SalesOrderId = valid_induk1.Id,
                    Quantity = jumlah1
                };

                valid_anak2 = new SalesOrderDetail()
                {
                    ItemId = item.Id,
                    SalesOrderId = valid_induk2.Id,
                    Quantity = jumlah2
                };

                invalid_anak1 = new SalesOrderDetail()
                {
                    SalesOrderId = valid_induk1.Id,
                    Quantity = jumlah1
                };

                invalid_anak2 = new SalesOrderDetail()
                {
                    ItemId = item.Id,
                    Quantity = jumlah1
                };

                invalid_anak3 = new SalesOrderDetail()
                {
                    ItemId = item.Id,
                    SalesOrderId = valid_induk1.Id,
                };
            }
        }

        /*
        * STEPS:
        * 1.    create_salesorder_with_no_contact
        * 2.    create_salesorder_with_no_salesdate
        * 3.    create_valid_salesorder
        * 4.    update_salesorder_with_no_contact
        * 5.    update_valid_salesorder
        * 6.    confirm_salesorder_with_no_salesorderdetails
        * 7.    delete_unconfirmed_salesorder
        * 8.    delete_confirmed_salesorder
        * 9a.       create_salesorderdetail_with_no_item
        * 9b.       create_salesorderdetail_with_no_salesorder
        * 9c.       create_salesorderdetail_with_no_quantity
        * 9d.       create_salesorderdetail_with_same_item
        * 9e.       create_valid_salesorderdetail
        * 3f1.          confirm_salesorder_with_invalid_salesorderdetails_quantity
        * 3f2.          confirm_valid_salesorder
        * 3f3.          unconfirm_valid_salesorderdetail
        * 3f4.          delete_confirmed_salesorderdetail
        * 3f5.          delete_unconfirmed_salesorderdetail 
        */
        void salesorder_validation()
        {
            it["create_salesorder_with_no_contact"] = () =>
            {
                invalid_induk1.Errors.Count().should_not_be(0);
            };

            it["create_salesorder_with_no_salesdate"] = () =>
            {
                invalid_induk2.Errors.Count().should_not_be(0);
            };

            it["create_valid_salesorder"] = () =>
            {
                valid_induk1.Errors.Count().should_be(0);
                valid_induk1.ContactId.should_be(contact.Id);
                valid_induk1.SalesDate.should_be(now);
                valid_induk1.CreatedAt.should_not_be(null);
                valid_induk1.IsDeleted.should_be_false();
                valid_induk1.IsConfirmed.should_be_false();
                valid_induk1.Code.should_not_be_empty();
            };

            it["update_salesorder_with_no_contact"] = () =>
            {
                valid_induk1.ContactId = 0; // 0 = invalid id
                valid_induk1.SalesDate = DateTime.Now;
                _salesOrderService.UpdateObject(valid_induk1, _contactService);
                valid_induk1.Errors.Count().should_not_be(0);
            };

            it["update_valid_salesorder"] = () =>
            {
                DateTime now2 = DateTime.Now;

                valid_induk1.ContactId = contact2.Id;
                valid_induk1.SalesDate = now2;
                _salesOrderService.UpdateObject(valid_induk1, _contactService);

                contact2.Errors.Count().should_be(0);
                valid_induk1.Errors.Count().should_be(0);
                valid_induk1.ContactId.should_be(contact2.Id);
                valid_induk1.SalesDate.should_be(now2);
                valid_induk1.CreatedAt.should_not_be(null);
                valid_induk1.IsDeleted.should_be_false();
                valid_induk1.IsConfirmed.should_be_false();
                valid_induk1.Code.should_not_be_empty();
            };

            it["delete_unconfirmed_salesorder"] = () =>
            {
                _salesOrderService.SoftDeleteObject(valid_induk1, _salesOrderDetailService);
                valid_induk1.Errors.Count().should_be(0);
            };

            it["delete_confirmed_salesorder"] = () =>
            {
                _salesOrderDetailService.CreateObject(valid_anak1, _salesOrderService, _itemService);
                
                _salesOrderService.ConfirmObject(valid_induk1, _salesOrderDetailService, _stockMutationService, _itemService);
                
                _salesOrderService.SoftDeleteObject(valid_induk1, _salesOrderDetailService);
                valid_anak1.Errors.Count().should_be(0);
                valid_induk1.Errors.Count().should_not_be(0);
            };

            it["confirm_salesorder_with_no_salesorderdetails"] = () =>
            {
                _salesOrderService.ConfirmObject(valid_induk1, _salesOrderDetailService, _stockMutationService, _itemService);
                valid_induk1.Errors.Count().should_not_be(0);
                valid_induk1.IsConfirmed.should_be_false();
            };

            context["ketika_induk_sudah_terbuat"] = () =>
            {
                before = () =>
                {
                    _salesOrderDetailService.CreateObject(invalid_anak1, _salesOrderService, _itemService);
                    _salesOrderDetailService.CreateObject(invalid_anak2, _salesOrderService, _itemService);
                    _salesOrderDetailService.CreateObject(invalid_anak3, _salesOrderService, _itemService);
                };  

                it["create_salesorderdetail_with_no_item"] = () =>
                {
                    invalid_anak1.Errors.Count().should_not_be(0);
                };

                it["create_salesorderdetail_with_no_salesorder"] = () =>
                {
                    invalid_anak2.Errors.Count().should_not_be(0);
                };

                it["create_salesorderdetail_with_no_quantity"] = () =>
                {
                    invalid_anak3.Errors.Count().should_not_be(0);
                };

                it["create_salesorderdetail_with_same_item"] = () =>
                {
                    _salesOrderDetailService.CreateObject(valid_anak1, _salesOrderService, _itemService);
                    _salesOrderDetailService.CreateObject(valid_anak1, _salesOrderService, _itemService);
                    valid_anak1.Errors.Count().should_not_be(0);
                };

                it["create_valid_salesorderdetail"] = () =>
                {
                    _salesOrderDetailService.CreateObject(valid_anak1, _salesOrderService, _itemService);
                    valid_anak1.Errors.Count().should_be(0);
                    valid_anak1.Code.should_not_be_empty();
                };

                // Custom validation
                context["ketika_anak_sudah_terbuat"] = () =>
                {
                    before = () =>
                    {
                        _salesOrderDetailService.CreateObject(valid_anak1, _salesOrderService, _itemService);
                        _salesOrderDetailService.CreateObject(valid_anak2, _salesOrderService, _itemService);
                    };

                    it["confirm_salesorder_with_invalid_salesorderdetails_quantity"] = () =>
                    {
                        _salesOrderService.ConfirmObject(valid_induk2, _salesOrderDetailService, _stockMutationService, _itemService);
                        valid_anak2.Errors.Count().should_not_be(0);
                        valid_anak2.IsConfirmed.should_be_false();
                        valid_induk2.Errors.Count().should_not_be(0);
                        valid_induk2.IsConfirmed.should_be_false();
                    };

                    it["confirm_valid_salesorder"] = () =>
                    {
                        _salesOrderService.ConfirmObject(valid_induk1, _salesOrderDetailService, _stockMutationService, _itemService);
                        Item i = _itemService.GetObjectById(valid_anak1.ItemId);
                        i.PendingDelivery.should_be(valid_anak1.Quantity); //jumlah1
                        IList<StockMutation> sms = _stockMutationService.GetObjectsByAllIds(valid_anak1.ItemId, valid_anak1.Id, "SalesOrderDetail");
                        sms.Count().should_be(1);
                        foreach (var sm in sms)
                        {
                            sm.Status.should_be("Addition");
                            sm.Quantity.should_be(valid_anak1.Quantity);
                        }
                        valid_induk1.Errors.Count().should_be(0);
                        valid_induk1.IsConfirmed.should_be_true();
                        valid_induk1.ConfirmationDate.should_not_be(null);
                    };

                    it["unconfirm_valid_salesorderdetail"] = () =>
                    {
                        _salesOrderService.ConfirmObject(valid_induk1, _salesOrderDetailService, _stockMutationService, _itemService);
                        valid_anak1.IsConfirmed.should_be_true();
                        _salesOrderService.UnconfirmObject(valid_induk1, _salesOrderDetailService, _stockMutationService, _itemService, _deliveryOrderDetailService);
                        Item i = _itemService.GetObjectById(valid_anak1.ItemId);
                        i.PendingDelivery.should_be(0); //jumlah1
                        IList<StockMutation> sms = _stockMutationService.GetObjectsByAllIds(valid_anak1.ItemId, valid_anak1.Id, "SalesOrderDetail");
                        sms.Count().should_be(0);
                        foreach (var sm in sms)
                        {
                            sm.IsDeleted.should_be_true();
                        }
                        valid_induk1.Errors.Count().should_be(0);
                        valid_induk1.IsConfirmed.should_be_false();
                        
                        
                    };

                    it["delete_confirmed_salesorderdetail"] = () =>
                    {
                        _salesOrderService.ConfirmObject(valid_induk1, _salesOrderDetailService, _stockMutationService, _itemService);
                        valid_anak1.IsConfirmed.should_be_true();
                        _salesOrderDetailService.SoftDeleteObject(valid_anak1);
                        valid_anak1.Errors.Count().should_not_be(0);
                    };

                    it["delete_unconfirmed_salesorderdetail"] = () =>
                    {
                        _salesOrderDetailService.SoftDeleteObject(valid_anak1);
                        valid_anak1.Errors.Count().should_be(0);
                    };
                };
            };
        }
    }
}
