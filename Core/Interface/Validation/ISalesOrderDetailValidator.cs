using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DomainModel;
using Core.Interface.Service;

namespace Core.Interface.Validation
{
    public interface ISalesOrderDetailValidator
    {
        SalesOrderDetail VHasSalesOrder(SalesOrderDetail salesOrderDetail);
        SalesOrderDetail VHasItem(SalesOrderDetail salesOrderDetail);
        SalesOrderDetail VIsNotConfirmed(SalesOrderDetail salesOrderDetail);
        SalesOrderDetail VIsPositiveQuantity(SalesOrderDetail salesOrderDetail);
        SalesOrderDetail VIsQuantityValid(SalesOrderDetail salesOrderDetail, IItemService _itemService);
        SalesOrderDetail VIsUnconfirmQuantityValid(SalesOrderDetail salesOrderDetail, IItemService _itemService);
        SalesOrderDetail VIsItemUnique(SalesOrderDetail salesOrderDetail, ISalesOrderDetailService _salesOrderDetailService);
        SalesOrderDetail VDontHaveDeliveryOrderDetails(SalesOrderDetail salesOrderDetail, IDeliveryOrderDetailService _deliveryOrderDetailService);
        SalesOrderDetail VCreateObject(SalesOrderDetail salesOrderDetail, ISalesOrderDetailService _salesOrderDetailService, IItemService _itemService);
        SalesOrderDetail VUpdateObject(SalesOrderDetail salesOrderDetail, ISalesOrderDetailService _salesOrderDetailService, IItemService _itemService);
        SalesOrderDetail VDeleteObject(SalesOrderDetail salesOrderDetail, ISalesOrderDetailService _salesOrderDetailService);
        SalesOrderDetail VUnconfirmObject(SalesOrderDetail salesOrderDetail, IItemService _itemService, IDeliveryOrderDetailService _deliveryOrderDetailService);
        SalesOrderDetail VReceiveObject(SalesOrderDetail salesOrderDetail);

        bool ValidCreateObject(SalesOrderDetail salesOrderDetail, ISalesOrderDetailService _salesOrderDetailService, IItemService _itemService);
        bool ValidUpdateObject(SalesOrderDetail salesOrderDetail, ISalesOrderDetailService _salesOrderDetailService, IItemService _itemService);
        bool ValidDeleteObject(SalesOrderDetail salesOrderDetail, ISalesOrderDetailService _salesOrderDetailService);
        bool ValidConfirmObject(SalesOrderDetail salesOrderDetail, IItemService _itemService);
        bool ValidUnconfirmObject(SalesOrderDetail salesOrderDetail, IItemService _itemService, IDeliveryOrderDetailService _deliveryOrderDetailService);
        bool ValidReceiveObject(SalesOrderDetail salesOrderDetail);
        bool isValid(SalesOrderDetail obj);
        string PrintError(SalesOrderDetail obj);
    }
}
