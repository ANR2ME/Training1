using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DomainModel;
using Core.Interface.Service;

namespace Core.Interface.Validation
{
    public interface IDeliveryOrderDetailValidator
    {
        DeliveryOrderDetail VHasDeliveryOrder(DeliveryOrderDetail deliveryOrderDetail);
        DeliveryOrderDetail VHasItem(DeliveryOrderDetail deliveryOrderDetail);
        DeliveryOrderDetail VHasSalesOrderDetail(DeliveryOrderDetail deliveryOrderDetail);
        DeliveryOrderDetail VIsNotConfirmed(DeliveryOrderDetail deliveryOrderDetail);
        DeliveryOrderDetail VIsConfirmed(DeliveryOrderDetail deliveryOrderDetail);
        DeliveryOrderDetail VIsPositiveQuantity(DeliveryOrderDetail deliveryOrderDetail);
        DeliveryOrderDetail VIsQuantityValid(DeliveryOrderDetail deliveryOrderDetail, IItemService _itemService);
        DeliveryOrderDetail VIsUnconfirmQuantityValid(DeliveryOrderDetail deliveryOrderDetail, IItemService _itemService);
        DeliveryOrderDetail VIsItemUnique(DeliveryOrderDetail deliveryOrderDetail, IDeliveryOrderDetailService _deliveryOrderDetailService);
        DeliveryOrderDetail VIsValidOrderQuantity(DeliveryOrderDetail deliveryOrderDetail, ISalesOrderDetailService _salesOrderDetailService);
        DeliveryOrderDetail VIsOrderDetailConfirmed(DeliveryOrderDetail deliveryOrderDetail, ISalesOrderDetailService _salesOrderDetailService);
        DeliveryOrderDetail VCreateObject(DeliveryOrderDetail deliveryOrderDetail, IDeliveryOrderDetailService _deliveryOrderDetailService, ISalesOrderDetailService _salesOrderDetailService);
        DeliveryOrderDetail VUpdateObject(DeliveryOrderDetail deliveryOrderDetail, IDeliveryOrderDetailService _deliveryOrderDetailService);
        DeliveryOrderDetail VDeleteObject(DeliveryOrderDetail deliveryOrderDetail, IDeliveryOrderDetailService _deliveryOrderDetailService);
        DeliveryOrderDetail VUnconfirmObject(DeliveryOrderDetail deliveryOrderDetail, IItemService _itemService);

        bool ValidCreateObject(DeliveryOrderDetail deliveryOrderDetail, IDeliveryOrderDetailService _deliveryOrderDetailService, ISalesOrderDetailService _salesOrderDetailService);
        bool ValidUpdateObject(DeliveryOrderDetail deliveryOrderDetail, IDeliveryOrderDetailService _deliveryOrderDetailService);
        bool ValidDeleteObject(DeliveryOrderDetail deliveryOrderDetail, IDeliveryOrderDetailService _deliveryOrderDetailService);
        bool ValidConfirmObject(DeliveryOrderDetail deliveryOrderDetail, IItemService _itemService, ISalesOrderDetailService _salesOrderDetailService);
        bool ValidUnconfirmObject(DeliveryOrderDetail deliveryOrderDetail, IItemService _itemService);
        bool isValid(DeliveryOrderDetail obj);
        string PrintError(DeliveryOrderDetail obj);
    }
}
