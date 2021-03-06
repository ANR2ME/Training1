﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DomainModel;
using Core.Interface.Service;

namespace Core.Interface.Validation
{
    public interface IPurchaseOrderDetailValidator
    {
        PurchaseOrderDetail VHasPurchaseOrder(PurchaseOrderDetail purchaseOrderDetail, IPurchaseOrderService _purchaseOrderService);
        PurchaseOrderDetail VHasItem(PurchaseOrderDetail purchaseOrderDetail, IItemService _itemService);
        PurchaseOrderDetail VIsNotConfirmed(PurchaseOrderDetail purchaseOrderDetail);
        PurchaseOrderDetail VIsPositiveQuantity(PurchaseOrderDetail purchaseOrderDetail);
        //PurchaseOrderDetail VIsConfirmQuantityValid(PurchaseOrderDetail purchaseOrderDetail, IItemService _itemService);
        PurchaseOrderDetail VIsUnconfirmQuantityValid(PurchaseOrderDetail purchaseOrderDetail, IItemService _itemService);
        PurchaseOrderDetail VIsItemUnique(PurchaseOrderDetail purchaseOrderDetail, IPurchaseOrderDetailService _purchaseOrderDetailService);
        PurchaseOrderDetail VDontHavePurchaseReceivalDetails(PurchaseOrderDetail purchaseOrderDetail, IPurchaseReceivalDetailService _purchaseReceivalDetailService);
        PurchaseOrderDetail VCreateObject(PurchaseOrderDetail purchaseOrderDetail, IPurchaseOrderDetailService _purchaseOrderDetailService, IPurchaseOrderService _purchaseOrderService, IItemService _itemService);
        PurchaseOrderDetail VUpdateObject(PurchaseOrderDetail purchaseOrderDetail, IPurchaseOrderDetailService _purchaseOrderDetailService, IPurchaseOrderService _purchaseOrderService, IItemService _itemService);
        PurchaseOrderDetail VDeleteObject(PurchaseOrderDetail purchaseOrderDetail, IPurchaseOrderDetailService _purchaseOrderDetailService);
        PurchaseOrderDetail VUnconfirmObject(PurchaseOrderDetail purchaseOrderDetail, IItemService _itemService, IPurchaseReceivalDetailService _purchaseReceivalDetailService);
        PurchaseOrderDetail VReceiveObject(PurchaseOrderDetail purchaseOrderDetail);

        bool ValidCreateObject(PurchaseOrderDetail purchaseOrderDetail, IPurchaseOrderDetailService _purchaseOrderDetailService, IPurchaseOrderService _purchaseOrderService, IItemService _itemService);
        bool ValidUpdateObject(PurchaseOrderDetail purchaseOrderDetail, IPurchaseOrderDetailService _purchaseOrderDetailService, IPurchaseOrderService _purchaseOrderService, IItemService _itemService);
        bool ValidDeleteObject(PurchaseOrderDetail purchaseOrderDetail, IPurchaseOrderDetailService _purchaseOrderDetailService);
        bool ValidConfirmObject(PurchaseOrderDetail purchaseOrderDetail, IItemService _itemService);
        bool ValidUnconfirmObject(PurchaseOrderDetail purchaseOrderDetail, IItemService _itemService, IPurchaseReceivalDetailService _purchaseReceivalDetailService);
        bool ValidReceiveObject(PurchaseOrderDetail purchaseOrderDetail);
        bool isValid(PurchaseOrderDetail obj);
        string PrintError(PurchaseOrderDetail obj);
    }
}
