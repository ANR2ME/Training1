using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DomainModel;
using Core.Interface.Validation;

namespace Core.Interface.Service
{
    public interface IDeliveryOrderService
    {
        IDeliveryOrderValidator GetValidator();
        IList<DeliveryOrder> GetAll();

        DeliveryOrder GetObjectById(int Id);
        DeliveryOrder CreateObject(DeliveryOrder deliveryOrder, ISalesOrderService _salesOrderService);
        DeliveryOrder UpdateObject(DeliveryOrder deliveryOrder, ISalesOrderService _salesOrderService);
        DeliveryOrder ConfirmObject(DeliveryOrder deliveryOrder, IDeliveryOrderDetailService _deliveryOrderDetailService, IStockMutationService _stockMutationService, IItemService _itemService, ISalesOrderDetailService _salesOrderDetailService);
        DeliveryOrder UnconfirmObject(DeliveryOrder deliveryOrder, IDeliveryOrderDetailService _deliveryOrderDetailService, IStockMutationService _stockMutationService, IItemService _itemService);
        DeliveryOrder SoftDeleteObject(DeliveryOrder deliveryOrder, IDeliveryOrderDetailService _deliveryOrderDetailService);
        bool DeleteObject(int Id);
    }
}
