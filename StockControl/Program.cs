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
                IItemService iis = new ItemService(new ItemRepository(), new ItemValidator());
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
                isads.CreateObject(sad, isas);
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
                    CustomerId = 1,
                    PurchaseDate = DateTime.Now,
                };
                ipos.CreateObject(po);
                Console.WriteLine("PurchaseOrder Id:{0}, Date:{1}, Code:{2}, Error:{3}", po.Id, po.PurchaseDate.ToString(), po.Code, po.Errors.FirstOrDefault());

                IPurchaseOrderDetailService ipods = new PurchaseOrderDetailService(new PurchaseOrderDetailRepository(), new PurchaseOrderDetailValidator());
                PurchaseOrderDetail pod = new PurchaseOrderDetail()
                {
                    ItemId = item.Id,
                    PurchaseOrderId = po.Id,
                    Quantity = 20
                };
                ipods.CreateObject(pod, ipos);
                Console.WriteLine("PurchaseOrderDetail Id:{0}, ItemId:{1}, poId:{2}, Quantity:{3}, Code:{4}, Error:{5}", pod.Id, pod.ItemId, pod.PurchaseOrderId, pod.Quantity, pod.Code, pod.Errors.FirstOrDefault());
                ipos.ConfirmObject(po, ipods, isms, iis);
                Console.WriteLine("PurchaseOrder Confirmed Id:{0}, CustomerId:{1}, Error:{2}", po.Id, po.CustomerId, po.Errors.FirstOrDefault());
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
                IPurchaseReceivalDetailService iprds = new PurchaseReceivalDetailService(new PurchaseReceivalDetailRepository(), new PurchaseReceivalDetailValidator());
                //ipos.UnconfirmObject(po, ipods, isms, iis, iprds);
                //Console.WriteLine("PurchaseOrder UnConfirmed Id:{0}, CustomerId:{1}, Error:{2}", po.Id, po.CustomerId, po.Errors.FirstOrDefault());
                //Console.WriteLine("Press any key to continue...");
                //Console.ReadKey();

                IPurchaseReceivalService iprs = new PurchaseReceivalService(new PurchaseReceivalRepository(), new PurchaseReceivalValidator());
                PurchaseReceival pr = new PurchaseReceival()
                {
                    CustomerId = 1,
                    ReceivalDate = DateTime.Now,
                };
                iprs.CreateObject(pr);
                Console.WriteLine("PurchaseReceival Id:{0}, Date:{1}, Code:{2}, Error:{3}", pr.Id, pr.ReceivalDate.ToString(), pr.Code, pr.Errors.FirstOrDefault());

                //IPurchaseReceivalDetailService ipods = new PurchaseReceivalDetailService(new PurchaseOrderDetailRepository(), new PurchaseOrderDetailValidator());
                PurchaseReceivalDetail prd = new PurchaseReceivalDetail()
                {
                    ItemId = item.Id,
                    PurchaseReceivalId = pr.Id,
                    PurchaseOrderDetailId = pod.Id,
                    Quantity = 20
                };
                iprds.CreateObject(prd, iprs, ipods);
                Console.WriteLine("PurchaseReceivalDetail Id:{0}, ItemId:{1}, podId:{2}, Quantity:{3}, Code:{4}, Error:{5}", prd.Id, prd.ItemId, prd.PurchaseOrderDetailId, prd.Quantity, prd.Code, prd.Errors.FirstOrDefault());
                iprs.ConfirmObject(pr, iprds, isms, iis, ipods);
                Console.WriteLine("PurchaseReceival Confirmed Id:{0}, CustomerId:{1}, Error:{2}", pr.Id, pr.CustomerId, pr.Errors.FirstOrDefault());
                //Console.WriteLine("Press any key to Unconfirm...");
                //Console.ReadKey();
                //iprs.UnconfirmObject(pr, iprds, isms, iis);
                //Console.WriteLine("PurchaseReceival UnConfirmed Id:{0}, CustomerId:{1}, Error:{2}", pr.Id, pr.CustomerId, pr.Errors.FirstOrDefault());
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();

                ISalesOrderService isos = new SalesOrderService(new SalesOrderRepository(), new SalesOrderValidator());
                SalesOrder so = new SalesOrder()
                {
                    CustomerId = 1,
                    SalesDate = DateTime.Now
                };
                isos.CreateObject(so);
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
                Console.WriteLine("SalesOrder Confirmed Id:{0}, CustomerId:{1}, Error:{2}", so.Id, so.CustomerId, so.Errors.FirstOrDefault());
                //Console.WriteLine("Press any key to Unconfirm...");
                //Console.ReadKey();
                IDeliveryOrderDetailService idods = new DeliveryOrderDetailService(new DeliveryOrderDetailRepository(), new DeliveryOrderDetailValidator());
                //isos.UnconfirmObject(so,isods,isms,iis,idods);
                //Console.WriteLine("SalesOrder UnConfirmed Id:{0}, CustomerId:{1}, Error:{2}", so.Id, so.CustomerId, so.Errors.FirstOrDefault());
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
                IDeliveryOrderService idos = new DeliveryOrderService(new DeliveryOrderRepository(), new DeliveryOrderValidator());
                DeliveryOrder do2 = new DeliveryOrder()
                {
                    CustomerId = 1,
                    DeliveryDate = DateTime.Now,
                };
                idos.CreateObject(do2);
                Console.WriteLine("DeliveryOrder Id:{0}, Date:{1}, Code:{2}, Error:{3}", do2.Id, do2.DeliveryDate.ToString(), do2.Code, do2.Errors.FirstOrDefault());

                DeliveryOrderDetail dod = new DeliveryOrderDetail()
                {
                    ItemId = item.Id,
                    DeliveryOrderId = do2.Id,
                    SalesOrderDetailId = sod.Id,
                    Quantity = 20
                };
                idods.CreateObject(dod, idos, isods);
                Console.WriteLine("DeliveryOrderDetail Id:{0}, ItemId:{1}, podId:{2}, Quantity:{3}, Code:{4}, Error:{5}", dod.Id, dod.ItemId, dod.SalesOrderDetailId, dod.Quantity, dod.Code, dod.Errors.FirstOrDefault());
                idos.ConfirmObject(do2, idods, isms, iis, isods);
                Console.WriteLine("DeliveryOrderDetail Confirmed Id:{0}, CustomerId:{1}, Error:{2}", do2.Id, do2.CustomerId, do2.Errors.FirstOrDefault());
                Console.WriteLine("Press any key to Unconfirm...");
                Console.ReadKey();
                idos.UnconfirmObject(do2, idods, isms, iis);
                Console.WriteLine("DeliveryOrderDetail UnConfirmed Id:{0}, CustomerId:{1}, Error:{2}", do2.Id, do2.CustomerId, do2.Errors.FirstOrDefault());
                
                Console.WriteLine("Press any key to stop...");
                Console.ReadKey();
            }
        }
    }
}
