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
    public class SpecDeliveryOrder : nspec
    {
        int jumlah1, jumlah2;
        DateTime now;
        Contact contact, contact2;
        Item item;
        DeliveryOrder valid_induk1, valid_induk2, invalid_induk1, invalid_induk2;
        DeliveryOrderDetail valid_anak1, valid_anak2, invalid_anak1, invalid_anak2, invalid_anak3, invalid_anak4;
        SalesOrder valid_sinduk1;
        SalesOrderDetail valid_sanak1;

        IItemService _itemService;
        IStockMutationService _stockMutationService;
        IContactService _contactService;
        IDeliveryOrderDetailService _deliveryOrderDetailService;
        IDeliveryOrderService _deliveryOrderService;
        ISalesOrderService _salesOrderService;
        ISalesOrderDetailService _salesOrderDetailService;

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

                valid_sinduk1 = new SalesOrder()
                {
                    ContactId = contact.Id,
                    SalesDate = now
                };
                _salesOrderService.CreateObject(valid_sinduk1, _contactService);

                valid_sanak1 = new SalesOrderDetail()
                {
                    ItemId = item.Id,
                    SalesOrderId = valid_sinduk1.Id,
                    Quantity = jumlah1
                };
                _salesOrderDetailService.CreateObject(valid_sanak1, _salesOrderService, _itemService);

                valid_induk1 = new DeliveryOrder()
                {
                    SalesOrderId = valid_sinduk1.Id,
                    DeliveryDate = now
                };
                _deliveryOrderService.CreateObject(valid_induk1, _salesOrderService, _contactService);

                valid_induk2 = new DeliveryOrder()
                {
                    SalesOrderId = valid_sinduk1.Id,
                    DeliveryDate = now
                };
                _deliveryOrderService.CreateObject(valid_induk2, _salesOrderService, _contactService);

                invalid_induk1 = new DeliveryOrder()
                {
                    DeliveryDate = now
                };
                _deliveryOrderService.CreateObject(invalid_induk1, _salesOrderService, _contactService);

                invalid_induk2 = new DeliveryOrder()
                {
                    SalesOrderId = valid_sinduk1.Id
                };
                _deliveryOrderService.CreateObject(invalid_induk2, _salesOrderService, _contactService);

                valid_anak1 = new DeliveryOrderDetail()
                {
                    ItemId = item.Id,
                    DeliveryOrderId = valid_induk1.Id,
                    SalesOrderDetailId = valid_sanak1.Id,
                    Quantity = jumlah1
                };

                valid_anak2 = new DeliveryOrderDetail()
                {
                    ItemId = item.Id,
                    DeliveryOrderId = valid_induk2.Id,
                    SalesOrderDetailId = valid_sanak1.Id,
                    Quantity = jumlah2
                };

                invalid_anak1 = new DeliveryOrderDetail()
                {
                    DeliveryOrderId = valid_induk1.Id,
                    SalesOrderDetailId = valid_sanak1.Id,
                    Quantity = jumlah1
                };

                invalid_anak2 = new DeliveryOrderDetail()
                {
                    ItemId = item.Id,
                    SalesOrderDetailId = valid_sanak1.Id,
                    Quantity = jumlah1
                };

                invalid_anak3 = new DeliveryOrderDetail()
                {
                    ItemId = item.Id,
                    DeliveryOrderId = valid_induk1.Id,
                    SalesOrderDetailId = valid_sanak1.Id
                };

                invalid_anak4 = new DeliveryOrderDetail()
                {
                    ItemId = item.Id,
                    DeliveryOrderId = valid_induk2.Id,
                    Quantity = jumlah2
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
        void deliveryorder_validation()
        {
            it["create_deliveryorder_with_no_salesorder"] = () =>
            {
                invalid_induk1.Errors.Count().should_not_be(0);
            };

            it["create_deliveryorder_with_no_deliverydate"] = () =>
            {
                invalid_induk2.Errors.Count().should_not_be(0);
            };

            it["create_valid_deliveryorder"] = () =>
            {
                valid_induk1.Errors.Count().should_be(0);
                valid_induk1.Code.should_not_be_empty();
            };

            it["update_deliveryorder_with_no_salesorder"] = () =>
            {
                valid_induk1.SalesOrderId = 0;
                valid_induk1.DeliveryDate = DateTime.Now;

                _deliveryOrderService.UpdateObject(valid_induk1, _salesOrderService, _contactService);
                
                valid_induk1.Errors.Count().should_not_be(0);
            };

            it["update_valid_deliveryorder"] = () =>
            {
                DateTime now2 = DateTime.Now;

                valid_induk1.SalesOrderId = valid_sinduk1.Id;
                valid_induk1.DeliveryDate = now2;
                
                _deliveryOrderService.UpdateObject(valid_induk1, _salesOrderService, _contactService);

                valid_induk1.Errors.Count().should_be(0);
            };

            it["delete_unconfirmed_deliveryorder"] = () =>
            {
                _deliveryOrderService.SoftDeleteObject(valid_induk1, _deliveryOrderDetailService);
                valid_induk1.Errors.Count().should_be(0);
            };

            it["delete_confirmed_deliveryorder"] = () =>
            {
                _salesOrderService.ConfirmObject(valid_sinduk1, _salesOrderDetailService, _stockMutationService, _itemService);

                _deliveryOrderDetailService.CreateObject(valid_anak1, _itemService, _deliveryOrderService, _salesOrderDetailService);

                _deliveryOrderService.ConfirmObject(valid_induk1, _deliveryOrderDetailService, _stockMutationService, _itemService, _salesOrderDetailService);

                _deliveryOrderService.SoftDeleteObject(valid_induk1, _deliveryOrderDetailService);
                
                valid_anak1.Errors.Count().should_be(0);
                valid_induk1.Errors.Count().should_not_be(0);
            };

            it["confirm_deliveryorder_with_no_deliveryorderdetails"] = () =>
            {
                _deliveryOrderService.ConfirmObject(valid_induk1, _deliveryOrderDetailService, _stockMutationService, _itemService, _salesOrderDetailService);
                valid_induk1.Errors.Count().should_not_be(0);
                valid_induk1.IsConfirmed.should_be_false();
            };

            context["ketika_induk_sudah_terbuat"] = () =>
            {
                before = () =>
                {
                    _deliveryOrderDetailService.CreateObject(invalid_anak1, _itemService, _deliveryOrderService, _salesOrderDetailService);
                    _deliveryOrderDetailService.CreateObject(invalid_anak2, _itemService, _deliveryOrderService, _salesOrderDetailService);
                    _deliveryOrderDetailService.CreateObject(invalid_anak3, _itemService, _deliveryOrderService, _salesOrderDetailService);
                    _deliveryOrderDetailService.CreateObject(invalid_anak4, _itemService, _deliveryOrderService, _salesOrderDetailService);
                };

                it["create_deliveryorderdetail_with_no_item"] = () =>
                {
                    invalid_anak1.Errors.Count().should_not_be(0);
                };

                it["create_deliveryorderdetail_with_no_deliveryorder"] = () =>
                {
                    invalid_anak2.Errors.Count().should_not_be(0);
                };

                it["create_deliveryorderdetail_with_no_quantity"] = () =>
                {
                    invalid_anak3.Errors.Count().should_not_be(0);
                };

                it["create_deliveryorderdetail_with_no_salesorderdetail"] = () =>
                {
                    invalid_anak4.Errors.Count().should_not_be(0);
                };

                it["create_deliveryorderdetail_with_same_item"] = () =>
                {
                    _deliveryOrderDetailService.CreateObject(valid_anak1, _itemService, _deliveryOrderService, _salesOrderDetailService);
                    _deliveryOrderDetailService.CreateObject(valid_anak1, _itemService, _deliveryOrderService, _salesOrderDetailService);
                    valid_anak1.Errors.Count().should_not_be(0);
                };

                it["create_valid_deliveryorderdetail"] = () =>
                {
                    _deliveryOrderDetailService.CreateObject(valid_anak1, _itemService, _deliveryOrderService, _salesOrderDetailService);
                    valid_anak1.Errors.Count().should_be(0);
                    valid_anak1.Code.should_not_be_empty();
                };

                // Custom validation
                context["ketika_anak_sudah_terbuat"] = () =>
                {
                    before = () =>
                    {
                        _salesOrderService.ConfirmObject(valid_sinduk1, _salesOrderDetailService, _stockMutationService, _itemService);
                        
                        _deliveryOrderDetailService.CreateObject(valid_anak1, _itemService, _deliveryOrderService, _salesOrderDetailService);
                        _deliveryOrderDetailService.CreateObject(valid_anak2, _itemService, _deliveryOrderService, _salesOrderDetailService);
                    };

                    it["confirm_deliveryorder_with_invalid_quantity"] = () =>
                    {
                        _deliveryOrderService.ConfirmObject(valid_induk2, _deliveryOrderDetailService, _stockMutationService, _itemService, _salesOrderDetailService);
                        valid_anak2.Errors.Count().should_not_be(0);
                        valid_anak2.IsConfirmed.should_be_false();
                        valid_induk2.Errors.Count().should_not_be(0);
                        valid_induk2.IsConfirmed.should_be_false();
                    };

                    it["confirm_valid_deliveryorder"] = () =>
                    {
                        _deliveryOrderService.ConfirmObject(valid_induk1, _deliveryOrderDetailService, _stockMutationService, _itemService, _salesOrderDetailService);
                        Item i = _itemService.GetObjectById(valid_anak1.ItemId);
                        i.PendingDelivery.should_be(0); //jumlah1
                        i.Quantity.should_be(0);
                        IList<StockMutation> sms = _stockMutationService.GetObjectsByAllIds(valid_anak1.ItemId, valid_anak1.Id, "DeliveryOrderDetail");
                        sms.Count().should_be(2);
                        foreach (var sm in sms)
                        {
                            sm.Status.should_be("Deduction");
                            sm.Quantity.should_be(valid_anak1.Quantity);
                        }
                        valid_induk1.Errors.Count().should_be(0);
                        valid_induk1.IsConfirmed.should_be_true();
                        valid_induk1.ConfirmationDate.should_not_be(null);
                    };

                    it["unconfirm_valid_deliveryorderdetail"] = () =>
                    {
                        _deliveryOrderService.ConfirmObject(valid_induk1, _deliveryOrderDetailService, _stockMutationService, _itemService, _salesOrderDetailService);
                        valid_anak1.IsConfirmed.should_be_true();
                        _deliveryOrderService.UnconfirmObject(valid_induk1, _deliveryOrderDetailService, _stockMutationService, _itemService);
                        Item i = _itemService.GetObjectById(valid_anak1.ItemId);
                        i.PendingDelivery.should_be(valid_anak1.Quantity); //jumlah1
                        i.Quantity.should_be(valid_anak1.Quantity);
                        IList<StockMutation> sms = _stockMutationService.GetObjectsByAllIds(valid_anak1.ItemId, valid_anak1.Id, "DeliveryOrderDetail");
                        sms.Count().should_be(0);
                        foreach (var sm in sms)
                        {
                            sm.IsDeleted.should_be_true();
                        }
                        valid_induk1.Errors.Count().should_be(0);
                        valid_induk1.IsConfirmed.should_be_false();
                    };

                    it["delete_confirmed_deliveryorderdetail"] = () =>
                    {
                        _deliveryOrderService.ConfirmObject(valid_induk1, _deliveryOrderDetailService, _stockMutationService, _itemService, _salesOrderDetailService);
                        valid_anak1.IsConfirmed.should_be_true();
                        _deliveryOrderDetailService.SoftDeleteObject(valid_anak1);
                        valid_anak1.Errors.Count().should_not_be(0);
                    };

                    it["delete_unconfirmed_deliveryorderdetail"] = () =>
                    {
                        _deliveryOrderDetailService.SoftDeleteObject(valid_anak1);
                        valid_anak1.Errors.Count().should_be(0);
                    };
                };
            };
        }
    }
}
