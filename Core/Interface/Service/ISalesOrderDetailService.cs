using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DomainModel;
using Core.Interface.Validation;

namespace Core.Interface.Service
{
    public interface ISalesOrderDetailService
    {
        ISalesOrderDetailValidator GetValidator();
        IList<SalesOrderDetail> GetAll();
        IList<SalesOrderDetail> GetObjectsByItemId(int ItemId);
        IList<SalesOrderDetail> GetObjectsBySalesOrderId(int SalesOrderId);
        IList<SalesOrderDetail> GetConfirmedObjectsBySalesOrderId(int SalesOrderId);
        SalesOrderDetail GetObjectById(int Id);
        SalesOrderDetail CreateObject(SalesOrderDetail salesOrderDetail, ISalesOrderService _salesOrderService, IItemService _itemService);
        SalesOrderDetail UpdateObject(SalesOrderDetail salesOrderDetail, ISalesOrderService _salesOrderService, IItemService _itemService);
        SalesOrderDetail SoftDeleteObject(SalesOrderDetail salesOrderDetail);
        SalesOrderDetail ConfirmObject(SalesOrderDetail salesOrderDetail, IStockMutationService _stockMutationService, IItemService _itemService);
        SalesOrderDetail UnconfirmObject(SalesOrderDetail salesOrderDetail, IStockMutationService _stockMutationService, IItemService _itemService, IDeliveryOrderDetailService _deliveryOrderDetailService);
        bool DeleteObject(int Id);
    }
}
