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
    public class SpecContact : nspec
    {
        Contact contact;

        IContactService _contactService;
        IPurchaseOrderService _purchaseOrderService;
        ISalesOrderService _salesOrderService;

        void before_each()
        {
            var db = new StockControlEntities();
            using (db)
            {
                db.DeleteAllTables();
                _contactService = new ContactService(new ContactRepository(), new ContactValidator());
                _purchaseOrderService = new PurchaseOrderService(new PurchaseOrderRepository(), new PurchaseOrderValidator());
                _salesOrderService = new SalesOrderService(new SalesOrderRepository(), new SalesOrderValidator());

                contact = new Contact()
                {
                    Name = "Alfa Beta",
                    Address = "Jl.Panjang No.10",
                    PhoneNumber = "021-555-1234"
                };
                contact = _contactService.CreateObject(contact);
            }
        }

        /*
        * STEPS:
        * 1a. Create valid contact
        * 1b. Create [invalid] contact with no name
        * 1c. Create [invalid] contact with no address
        * 1d. Create [invalid] contact with no phonenumber
        * 2a. Update valid contact
        * 2b. Update [invalid] contact with no name
        * 2c. Update [invalid] contact with no address
        * 2d. Update [invalid] contact with no phonenumber
        * 3a. Delete contact
        * 3b. Delete contact with purchaseorders
        * 3c. Delete contact with salesorders
        */
        void contact_validation()
        {
            it["create_valid_contact"] = () =>
            {
                contact.Errors.Count().should_be(0);
            };

            it["create_contact_with_no_name"] = () =>
            {
                Contact nonamecontact = new Contact()
                {
                    Name = "",
                    Address = "Jl.Panjang No.10",
                    PhoneNumber = "021-555-1234"
                };
                nonamecontact = _contactService.CreateObject(nonamecontact);
                nonamecontact.Errors.Count().should_not_be(0);
            };

            it["create_contact_with_no_address"] = () =>
            {
                Contact nonamecontact = new Contact()
                {
                    Name = "Alfa Beta",
                    Address = "",
                    PhoneNumber = "021-555-1234"
                };
                nonamecontact = _contactService.CreateObject(nonamecontact);
                nonamecontact.Errors.Count().should_not_be(0);
            };

            it["create_contact_with_no_phoneumber"] = () =>
            {
                Contact sameskucontact = new Contact()
                {
                    Name = "Alfa Beta",
                    Address = "Jl.Panjang No.10",
                    PhoneNumber = ""
                };
                sameskucontact = _contactService.CreateObject(sameskucontact);
                sameskucontact.Errors.Count().should_not_be(0);
            };

            it["update_valid_contact"] = () =>
            {
                contact.Name = "Beta Gamma";
                contact.Address = "Jl.Pendek No.2";
                contact.PhoneNumber = "021-555-6666";
                _contactService.UpdateObject(contact);
                contact.Errors.Count().should_be(0);
            };

            it["update_contact_with_no_name"] = () =>
            {
                contact.Name = "";
                contact.Address = "Jl.Pendek No.2";
                contact.PhoneNumber = "021-555-6666";
                _contactService.UpdateObject(contact);
                contact.Errors.Count().should_not_be(0);
            };

            it["update_contact_with_no_address"] = () =>
            {
                contact.Name = "Beta Gamma";
                contact.Address = "";
                contact.PhoneNumber = "021-555-6666";
                _contactService.UpdateObject(contact);
                contact.Errors.Count().should_not_be(0);
            };

            it["update_contact_with_no_phonenumber"] = () =>
            {
                contact.Name = "Beta Gamma";
                contact.Address = "Jl.Pendek No.2";
                contact.PhoneNumber = "";
                _contactService.UpdateObject(contact);
                contact.Errors.Count().should_not_be(0);
            };

            it["delete_contact"] = () =>
            {
                contact = _contactService.SoftDeleteObject(contact, _purchaseOrderService, _salesOrderService);
                contact.Errors.Count().should_be(0);
            };

            it["delete_contact_with_purchaseorders"] = () =>
            {
                PurchaseOrder x = new PurchaseOrder()
                {
                    ContactId = contact.Id,
                    PurchaseDate = DateTime.Now,
                };
                x = _purchaseOrderService.CreateObject(x, _contactService);
                if (x.Errors.Count() > 0) Console.WriteLine("Error:{0}", x.Errors.FirstOrDefault());
                x.Errors.Count().should_be(0);

                contact = _contactService.SoftDeleteObject(contact, _purchaseOrderService, _salesOrderService);
                contact.Errors.Count().should_not_be(0);
            };

            it["delete_contact_with_salesorders"] = () =>
            {
                SalesOrder x = new SalesOrder()
                {
                    ContactId = contact.Id,
                    SalesDate = DateTime.Now,
                };
                x = _salesOrderService.CreateObject(x, _contactService);
                if (x.Errors.Count() > 0) Console.WriteLine("x.Error:{0}", x.Errors.FirstOrDefault());
                x.Errors.Count().should_be(0);

                contact = _contactService.SoftDeleteObject(contact, _purchaseOrderService, _salesOrderService);
                contact.Errors.Count().should_not_be(0);
            };
        }
    }
}
