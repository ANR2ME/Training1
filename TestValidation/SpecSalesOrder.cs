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
        SalesOrder salesOrder;

        IItemService _itemService;
        IStockMutationService _stockMutationService;
        IContactService _contactService;
        ISalesOrderDetailService _salesOrderDetailService;
        ISalesOrderService _salesOrderService;

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

                Contact contact = new Contact()
                {
                    Address = "Jl. XXX",
                    PhoneNumber = "021-3456",
                    Name = "Ade"
                };
                contact = _contactService.CreateObject(contact);

                salesOrder = new SalesOrder()
                {
                    ContactId = contact.Id,
                    SalesDate = DateTime.Now
                };
                salesOrder = _salesOrderService.CreateObject(salesOrder);
            }
        }

        /*
        * STEPS:
        * 1a. Create valid salesorder
        * 1b. Create [invalid] salesorder with no contact
        * 1c. Create [invalid] salesorder with no salesdate
        * 2a. Update valid salesorder
        * 2b. Update [invalid] salesorder with no contact
        * 3a. Delete salesorder
        * 3b. Delete confirmed salesorder
        */
        void salesorder_validation()
        {
            it["create_valid_salesorder"] = () =>
            {
                salesOrder.Errors.Count().should_be(0);
            };

            it["create_salesorder_with_no_contact"] = () =>
            {
                SalesOrder nocontactso = new SalesOrder()
                {
                    ContactId = 0,
                    SalesDate = DateTime.Now
                };
                nocontactso = _salesOrderService.CreateObject(nocontactso);
                nocontactso.Errors.Count().should_not_be(0);
            };

            it["create_salesorder_with_no_salesdate"] = () =>
            {
                SalesOrder nocontactso = new SalesOrder()
                {
                    ContactId = 1,
                };
                nocontactso = _salesOrderService.CreateObject(nocontactso);
                nocontactso.Errors.Count().should_not_be(0);
            };

            it["update_valid_salesorder"] = () =>
            {
                salesOrder.ContactId = 2;
                salesOrder.SalesDate = DateTime.Now;
                _salesOrderService.UpdateObject(salesOrder);
                salesOrder.Errors.Count().should_be(0);
            };

            it["update_salesorder_with_no_contact"] = () =>
            {
                salesOrder.ContactId = 0;
                salesOrder.SalesDate = DateTime.Now;
                _salesOrderService.UpdateObject(salesOrder);
                salesOrder.Errors.Count().should_not_be(0);
            };

            it["delete_salesorder"] = () =>
            {
                salesOrder = _salesOrderService.SoftDeleteObject(salesOrder, _salesOrderDetailService);
                salesOrder.Errors.Count().should_be(0);
            };

            it["delete_confirmed_salesorder"] = () =>
            {
                _salesOrderService.ConfirmObject(salesOrder, _salesOrderDetailService, _stockMutationService, _itemService);
                if (salesOrder.Errors.Count() > 0) Console.WriteLine("salesOrder.Error:{0}", salesOrder.Errors.FirstOrDefault());
                salesOrder = _salesOrderService.SoftDeleteObject(salesOrder, _salesOrderDetailService);
                salesOrder.Errors.Count().should_not_be(0);
            };
            
        }
    }
}
