using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DomainModel;
using Core.Interface.Validation;

namespace Core.Interface.Service
{
    public interface IDeliveryOrderDetailService
    {
        IDeliveryOrderDetailValidator GetValidator();
        IList<DeliveryOrderDetail> GetAll();
        IList<DeliveryOrderDetail> GetObjectsByDeliveryOrderId(int DeliveryOrderId);
        IList<DeliveryOrderDetail> GetConfirmedObjectsByDeliveryOrderId(int DeliveryOrderId);
        IList<DeliveryOrderDetail> GetObjectsBySalesOrderDetailId(int SalesOrderDetailId);
        DeliveryOrderDetail GetObjectById(int Id);
        DeliveryOrderDetail CreateObject(DeliveryOrderDetail deliveryOrderDetail, IDeliveryOrderService _deliveryOrderService, ISalesOrderDetailService _salesOrderDetailService);
        DeliveryOrderDetail UpdateObject(DeliveryOrderDetail deliveryOrderDetail);
        DeliveryOrderDetail SoftDeleteObject(DeliveryOrderDetail deliveryOrderDetail);
        DeliveryOrderDetail ConfirmObject(DeliveryOrderDetail deliveryOrderDetail, IStockMutationService _stockMutationService, IItemService _itemService, ISalesOrderDetailService _salesOrderDetailService);
        DeliveryOrderDetail UnconfirmObject(DeliveryOrderDetail deliveryOrderDetail, IStockMutationService _stockMutationService, IItemService _itemService);
        bool DeleteObject(int Id);
    }
}
