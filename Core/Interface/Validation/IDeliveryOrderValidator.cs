using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DomainModel;
using Core.Interface.Service;

namespace Core.Interface.Validation
{
    public interface IDeliveryOrderValidator
    {
        DeliveryOrder VHasSalesOrder(DeliveryOrder deliveryOrder, ISalesOrderService _salesOrderService);
        DeliveryOrder VHasContact(DeliveryOrder deliveryOrder, ISalesOrderService _salesOrderService, IContactService _contactService);
        DeliveryOrder VIsValidDeliveryDate(DeliveryOrder deliveryOrder);
        DeliveryOrder VIsConfirmed(DeliveryOrder deliveryOrder);
        DeliveryOrder VIsNotConfirmed(DeliveryOrder deliveryOrder);
        DeliveryOrder VIsDeliveryOrderDetailsNotConfirmed(DeliveryOrder deliveryOrder, IDeliveryOrderDetailService _deliveryOrderDetailService);
        DeliveryOrder VHasDeliveryOrderDetails(DeliveryOrder deliveryOrder, IDeliveryOrderDetailService _deliveryOrderDetailService);
        DeliveryOrder VIsValidDeliveryOrderDetailsQuantity(DeliveryOrder deliveryOrder, IDeliveryOrderDetailService _deliveryOrderDetailService, IItemService _itemService);
        DeliveryOrder VConfirmObject(DeliveryOrder deliveryOrder, IDeliveryOrderDetailService _deliveryOrderDetailService, IItemService _itemService);
        DeliveryOrder VUnconfirmObject(DeliveryOrder deliveryOrder);
        DeliveryOrder VCreateObject(DeliveryOrder deliveryOrder, ISalesOrderService _salesOrderService, IContactService _contactService);
        DeliveryOrder VUpdateObject(DeliveryOrder deliveryOrder, ISalesOrderService _salesOrderService, IContactService _contactService);
        DeliveryOrder VDeleteObject(DeliveryOrder deliveryOrder, IDeliveryOrderDetailService _deliveryOrderDetailService);

        bool ValidCreateObject(DeliveryOrder deliveryOrder, ISalesOrderService _salesOrderService, IContactService _contactService);
        bool ValidUpdateObject(DeliveryOrder deliveryOrder, ISalesOrderService _salesOrderService, IContactService _contactService);
        bool ValidDeleteObject(DeliveryOrder deliveryOrder, IDeliveryOrderDetailService _deliveryOrderDetailService);
        bool ValidConfirmObject(DeliveryOrder deliveryOrder, IDeliveryOrderDetailService _deliveryOrderDetailService, IItemService _itemService);
        bool ValidUnconfirmObject(DeliveryOrder deliveryOrder);
        bool isValid(DeliveryOrder obj);
        string PrintError(DeliveryOrder obj);
    }
}
