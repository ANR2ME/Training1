using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DomainModel;
using Core.Interface.Service;

namespace Core.Interface.Repository
{
    public interface IDeliveryOrderRepository : IRepository<DeliveryOrder>
    {
        IList<DeliveryOrder> GetAll();
        DeliveryOrder GetObjectById(int Id);
        DeliveryOrder CreateObject(DeliveryOrder deliveryOrder);
        DeliveryOrder UpdateObject(DeliveryOrder deliveryOrder);
        DeliveryOrder SoftDeleteObject(DeliveryOrder deliveryOrder);
        DeliveryOrder ConfirmObject(DeliveryOrder deliveryOrder, IDeliveryOrderDetailService _deliveryOrderDetailService, IStockMutationService _stockMutationService, IItemService _itemService, ISalesOrderDetailService _salesOrderDetailService);
        DeliveryOrder UnconfirmObject(DeliveryOrder deliveryOrder, IDeliveryOrderDetailService _deliveryOrderDetailService, IStockMutationService _stockMutationService, IItemService _itemService);
        bool DeleteObject(int Id);
        string SetObjectCode(DeliveryOrder obj);
    }
}
