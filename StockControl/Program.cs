using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Context;
using Core.DomainModel;
using Core.Interface.Service;
using Data.Repository;
using Service.Service;
using Validation.Validation;

namespace StockControl
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var db = new StockControlEntities();
            using (db)
            {
                int jumlah1, jumlah2;
                DateTime now;
                Contact contact, contact2;
                Item item;
                SalesOrder valid_induk1, valid_induk2, invalid_induk1, invalid_induk2;
                SalesOrderDetail valid_anak1, valid_anak2, invalid_anak1, invalid_anak2, invalid_anak3;

                IItemService _itemService;
                IStockMutationService _stockMutationService;
                IContactService _contactService;
                ISalesOrderDetailService _salesOrderDetailService;
                ISalesOrderService _salesOrderService;
                IDeliveryOrderService _deliveryOrderService;

                db.DeleteAllTables();
                _itemService = new ItemService(new ItemRepository(), new ItemValidator());
                _contactService = new ContactService(new ContactRepository(), new ContactValidator());
                _stockMutationService = new StockMutationService(new StockMutationRepository(), new StockMutationValidator());
                _salesOrderDetailService = new SalesOrderDetailService(new SalesOrderDetailRepository(), new SalesOrderDetailValidator());
                _salesOrderService = new SalesOrderService(new SalesOrderRepository(), new SalesOrderValidator());
                _deliveryOrderService = new DeliveryOrderService(new DeliveryOrderRepository(), new DeliveryOrderValidator());

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

                _salesOrderDetailService.CreateObject(invalid_anak1, _salesOrderService, _itemService);
                _salesOrderDetailService.CreateObject(invalid_anak2, _salesOrderService, _itemService);
                _salesOrderDetailService.CreateObject(invalid_anak3, _salesOrderService, _itemService);

                _salesOrderDetailService.CreateObject(valid_anak1, _salesOrderService, _itemService);
                _salesOrderDetailService.CreateObject(valid_anak1, _salesOrderService, _itemService);


                
                /*IItemService iis = new ItemService(new ItemRepository(), new ItemValidator());
                Item item = new Item()
                {
                    Sku = "tes001",
                    Description = "Buku",
                    Quantity = 0,
                    PendingDelivery = 0,
                    PendingReceival = 0
                };
                iis.CreateObject(item);
                Console.WriteLine("Item Id:{0}, Sku:{1}, Desc:{2}, Ready:{3}, PD:{4}, PR:{5}, Error:{6}", item.Id, item.Sku, item.Description, item.Quantity, item.PendingDelivery, item.PendingReceival, item.Errors.FirstOrDefault());

                IContactService ics = new ContactService(new ContactRepository(), new ContactValidator());
                Contact contact = new Contact()
                {
                    Name = "Alfa Beta",
                    Address = "Jl.Panjang No.10",
                    PhoneNumber = "021-555-1234"
                };
                ics.CreateObject(contact);
                Console.WriteLine("Contact Id:{0}, Name:{1}, Address:{2}, PhoneNumber:{3}, Error:{4}", contact.Id, contact.Name, contact.Address, contact.PhoneNumber, contact.Errors.FirstOrDefault());

                IStockAdjustmentService isas = new StockAdjustmentService(new StockAdjustmentRepository(), new StockAdjustmentValidator());
                StockAdjustment sa = new StockAdjustment()
                {
                    AdjustmentDate = DateTime.Now
                };
                isas.CreateObject(sa);
                Console.WriteLine("StockAdjustment Id:{0}, Date:{1}, Code:{2}, Error:{3}", sa.Id, sa.AdjustmentDate.ToString(), sa.Code, sa.Errors.FirstOrDefault());
                
                IStockAdjustmentDetailService isads = new StockAdjustmentDetailService(new StockAdjustmentDetailRepository(), new StockAdjustmentDetailValidator());
                StockAdjustmentDetail sad = new StockAdjustmentDetail()
                {
                    ItemId = item.Id,
                    StockAdjustmentId = sa.Id,
                    Quantity = 100
                };
                isads.CreateObject(sad, isas, iis);
                Console.WriteLine("StockAdjustmentDetail Id:{0}, ItemId:{1}, saId:{2}, Quantity:{3}, Code:{4}, Error:{5}", sad.Id, sad.ItemId, sad.StockAdjustmentId, sad.Quantity, sad.Code, sad.Errors.FirstOrDefault());
                IStockMutationService isms = new StockMutationService(new StockMutationRepository(), new StockMutationValidator());

                isas.ConfirmObject(sa, isads, isms, iis);
                Console.WriteLine("StockAdjustment Confirmed Id:{0}, Error:{1}", sa.Id, sa.Errors.FirstOrDefault());
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
                //isas.UnconfirmObject(sa, isads, isms, iis);
                //Console.WriteLine("StockAdjustment UnConfirmed Id:{0}, Error:{1}", sa.Id, sa.Errors.FirstOrDefault());
                //Console.WriteLine("Press any key to continue...");
                //Console.ReadKey();

                IPurchaseOrderService ipos = new PurchaseOrderService(new PurchaseOrderRepository(), new PurchaseOrderValidator());
                PurchaseOrder po = new PurchaseOrder()
                {
                    ContactId = 1,
                    PurchaseDate = DateTime.Now,
                };
                ipos.CreateObject(po, ics);
                Console.WriteLine("PurchaseOrder Id:{0}, Date:{1}, Code:{2}, Error:{3}", po.Id, po.PurchaseDate.ToString(), po.Code, po.Errors.FirstOrDefault());

                IPurchaseOrderDetailService ipods = new PurchaseOrderDetailService(new PurchaseOrderDetailRepository(), new PurchaseOrderDetailValidator());
                PurchaseOrderDetail pod = new PurchaseOrderDetail()
                {
                    ItemId = item.Id,
                    PurchaseOrderId = po.Id,
                    Quantity = 20
                };
                ipods.CreateObject(pod, ipos, iis);
                Console.WriteLine("PurchaseOrderDetail Id:{0}, ItemId:{1}, poId:{2}, Quantity:{3}, Code:{4}, Error:{5}", pod.Id, pod.ItemId, pod.PurchaseOrderId, pod.Quantity, pod.Code, pod.Errors.FirstOrDefault());
                ipos.ConfirmObject(po, ipods, isms, iis);
                Console.WriteLine("PurchaseOrder Confirmed Id:{0}, ContactId:{1}, Error:{2}", po.Id, po.ContactId, po.Errors.FirstOrDefault());
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
                IPurchaseReceivalDetailService iprds = new PurchaseReceivalDetailService(new PurchaseReceivalDetailRepository(), new PurchaseReceivalDetailValidator());
                //ipos.UnconfirmObject(po, ipods, isms, iis, iprds);
                //Console.WriteLine("PurchaseOrder UnConfirmed Id:{0}, ContactId:{1}, Error:{2}", po.Id, po.ContactId, po.Errors.FirstOrDefault());
                //Console.WriteLine("Press any key to continue...");
                //Console.ReadKey();

                IPurchaseReceivalService iprs = new PurchaseReceivalService(new PurchaseReceivalRepository(), new PurchaseReceivalValidator());
                PurchaseReceival pr = new PurchaseReceival()
                {
                    PurchaseOrderId = po.Id,
                    ReceivalDate = DateTime.Now,
                };
                iprs.CreateObject(pr, ipos, ics);
                Console.WriteLine("PurchaseReceival Id:{0}, Date:{1}, Code:{2}, Error:{3}", pr.Id, pr.ReceivalDate.ToString(), pr.Code, pr.Errors.FirstOrDefault());

                //IPurchaseReceivalDetailService ipods = new PurchaseReceivalDetailService(new PurchaseOrderDetailRepository(), new PurchaseOrderDetailValidator());
                PurchaseReceivalDetail prd = new PurchaseReceivalDetail()
                {
                    ItemId = item.Id,
                    PurchaseReceivalId = pr.Id,
                    PurchaseOrderDetailId = pod.Id,
                    Quantity = 20
                };
                iprds.CreateObject(prd, iprds, iprs, iis, ipods);
                Console.WriteLine("PurchaseReceivalDetail Id:{0}, ItemId:{1}, podId:{2}, Quantity:{3}, Code:{4}, Error:{5}", prd.Id, prd.ItemId, prd.PurchaseOrderDetailId, prd.Quantity, prd.Code, prd.Errors.FirstOrDefault());
                iprs.ConfirmObject(pr, iprds, isms, iis, ipods);
                Console.WriteLine("PurchaseReceival Confirmed Id:{0}, poId:{1}, Error:{2}", pr.Id, pr.PurchaseOrderId, pr.Errors.FirstOrDefault());
                //Console.WriteLine("Press any key to Unconfirm...");
                //Console.ReadKey();
                //iprs.UnconfirmObject(pr, iprds, isms, iis);
                //Console.WriteLine("PurchaseReceival UnConfirmed Id:{0}, ContactId:{1}, Error:{2}", pr.Id, pr.ContactId, pr.Errors.FirstOrDefault());
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();

                ISalesOrderService isos = new SalesOrderService(new SalesOrderRepository(), new SalesOrderValidator());
                SalesOrder so = new SalesOrder()
                {
                    ContactId = 1,
                    SalesDate = DateTime.Now
                };
                isos.CreateObject(so, ics);
                Console.WriteLine("SalesOrder Id:{0}, Date:{1}, Code:{2}, Error:{3}", so.Id, so.SalesDate.ToString(), so.Code, so.Errors.FirstOrDefault());

                ISalesOrderDetailService isods = new SalesOrderDetailService(new SalesOrderDetailRepository(), new SalesOrderDetailValidator());
                SalesOrderDetail sod = new SalesOrderDetail()
                {
                    ItemId = item.Id,
                    SalesOrderId = so.Id,
                    Quantity = 30
                };
                isods.CreateObject(sod, isos, iis);
                Console.WriteLine("SalesOrderDetail Id:{0}, ItemId:{1}, soId:{2}, Quantity:{3}, Code:{4}, Error:{5}", sod.Id, sod.ItemId, sod.SalesOrderId, sod.Quantity, sod.Code, sod.Errors.FirstOrDefault());
                isos.ConfirmObject(so, isods, isms, iis);
                Console.WriteLine("SalesOrder Confirmed Id:{0}, ContactId:{1}, Error:{2}", so.Id, so.ContactId, so.Errors.FirstOrDefault());
                //Console.WriteLine("Press any key to Unconfirm...");
                //Console.ReadKey();
                IDeliveryOrderDetailService idods = new DeliveryOrderDetailService(new DeliveryOrderDetailRepository(), new DeliveryOrderDetailValidator());
                //isos.UnconfirmObject(so,isods,isms,iis,idods);
                //Console.WriteLine("SalesOrder UnConfirmed Id:{0}, ContactId:{1}, Error:{2}", so.Id, so.ContactId, so.Errors.FirstOrDefault());
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
                IDeliveryOrderService idos = new DeliveryOrderService(new DeliveryOrderRepository(), new DeliveryOrderValidator());
                DeliveryOrder dor = new DeliveryOrder()
                {
                    SalesOrderId = so.Id,
                    DeliveryDate = DateTime.Now,
                };
                idos.CreateObject(dor, isos, ics);
                Console.WriteLine("DeliveryOrder Id:{0}, Date:{1}, Code:{2}, Error:{3}", dor.Id, dor.DeliveryDate.ToString(), dor.Code, dor.Errors.FirstOrDefault());

                DeliveryOrderDetail dod = new DeliveryOrderDetail()
                {
                    ItemId = item.Id,
                    DeliveryOrderId = dor.Id,
                    SalesOrderDetailId = sod.Id,
                    Quantity = 20
                };
                idods.CreateObject(dod, iis, idos, isods);
                Console.WriteLine("DeliveryOrderDetail Id:{0}, ItemId:{1}, podId:{2}, Quantity:{3}, Code:{4}, Error:{5}", dod.Id, dod.ItemId, dod.SalesOrderDetailId, dod.Quantity, dod.Code, dod.Errors.FirstOrDefault());
                idos.ConfirmObject(dor, idods, isms, iis, isods);
                Console.WriteLine("DeliveryOrderDetail Confirmed Id:{0}, soId:{1}, Error:{2}", dor.Id, dor.SalesOrderId, dor.Errors.FirstOrDefault());
                Console.WriteLine("Press any key to Unconfirm...");
                Console.ReadKey();
                idos.UnconfirmObject(dor, idods, isms, iis);
                Console.WriteLine("DeliveryOrderDetail UnConfirmed Id:{0}, soId:{1}, Error:{2}", dor.Id, dor.SalesOrderId, dor.Errors.FirstOrDefault());
                */
                Console.WriteLine("Press any key to stop...");
                Console.ReadKey();
            }
        }
    }
}
