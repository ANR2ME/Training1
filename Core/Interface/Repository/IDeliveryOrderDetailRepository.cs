using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DomainModel;
using Core.Interface.Service;

namespace Core.Interface.Repository
{
    public interface IDeliveryOrderDetailRepository : IRepository<DeliveryOrderDetail>
    {
        IList<DeliveryOrderDetail> GetAll();
        IList<DeliveryOrderDetail> GetObjectsByItemId(int ItemId);
        IList<DeliveryOrderDetail> GetObjectsByDeliveryOrderId(int deliveryOrderId);
        IList<DeliveryOrderDetail> GetConfirmedObjectsByDeliveryOrderId(int deliveryOrderId);
        IList<DeliveryOrderDetail> GetObjectsBySalesOrderDetailId(int SalesOrderDetailId);
        DeliveryOrderDetail GetObjectById(int Id);
        DeliveryOrderDetail CreateObject(DeliveryOrderDetail deliveryOrderDetail, string ParentCode);
        DeliveryOrderDetail UpdateObject(DeliveryOrderDetail deliveryOrderDetail);
        DeliveryOrderDetail SoftDeleteObject(DeliveryOrderDetail deliveryOrderDetail);
        DeliveryOrderDetail ConfirmObject(DeliveryOrderDetail deliveryOrderDetail, IStockMutationService _stockMutationService, IItemService _itemService);
        DeliveryOrderDetail UnconfirmObject(DeliveryOrderDetail deliveryOrderDetail, IStockMutationService _stockMutationService, IItemService _itemService);
        bool DeleteObject(int Id);
        string SetObjectCode(DeliveryOrderDetail obj, string ParentCode);
    }
}
