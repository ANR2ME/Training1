using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DomainModel;
using Core.Interface.Service;

namespace Core.Interface.Validation
{
    public interface IPurchaseReceivalDetailValidator
    {
        PurchaseReceivalDetail VHasPurchaseReceival(PurchaseReceivalDetail purchaseReceivalDetail);
        PurchaseReceivalDetail VHasItem(PurchaseReceivalDetail purchaseReceivalDetail);
        PurchaseReceivalDetail VHasPurchaseOrderDetail(PurchaseReceivalDetail purchaseReceivalDetail);
        PurchaseReceivalDetail VIsNotConfirmed(PurchaseReceivalDetail purchaseReceivalDetail);
        PurchaseReceivalDetail VIsConfirmed(PurchaseReceivalDetail purchaseReceivalDetail);
        PurchaseReceivalDetail VIsPositiveQuantity(PurchaseReceivalDetail purchaseReceivalDetail);
        //PurchaseReceivalDetail VIsConfirmQuantityValid(PurchaseReceivalDetail purchaseReceivalDetail, IItemService _itemService);
        PurchaseReceivalDetail VIsUnconfirmQuantityValid(PurchaseReceivalDetail purchaseReceivalDetail, IItemService _itemService);
        PurchaseReceivalDetail VIsItemUnique(PurchaseReceivalDetail purchaseReceivalDetail, IPurchaseReceivalDetailService _purchaseReceivalDetailService);
        PurchaseReceivalDetail VIsValidOrderQuantity(PurchaseReceivalDetail purchaseReceivalDetail, IPurchaseOrderDetailService _purchaseOrderDetailService);
        PurchaseReceivalDetail VIsOrderDetailConfirmed(PurchaseReceivalDetail purchaseReceivalDetail, IPurchaseOrderDetailService _purchaseOrderDetailService);
        PurchaseReceivalDetail VCreateObject(PurchaseReceivalDetail purchaseReceivalDetail, IPurchaseReceivalDetailService _purchaseReceivalDetailService, IPurchaseOrderDetailService _purchaseOrderDetailService);
        PurchaseReceivalDetail VUpdateObject(PurchaseReceivalDetail purchaseReceivalDetail, IPurchaseReceivalDetailService _purchaseReceivalDetailService);
        PurchaseReceivalDetail VDeleteObject(PurchaseReceivalDetail purchaseReceivalDetail, IPurchaseReceivalDetailService _purchaseReceivalDetailService);
        PurchaseReceivalDetail VUnconfirmObject(PurchaseReceivalDetail purchaseReceivalDetail, IItemService _itemService);

        bool ValidCreateObject(PurchaseReceivalDetail purchaseReceivalDetail, IPurchaseReceivalDetailService _purchaseReceivalDetailService, IPurchaseOrderDetailService _purchaseOrderDetailService);
        bool ValidUpdateObject(PurchaseReceivalDetail purchaseReceivalDetail, IPurchaseReceivalDetailService _purchaseReceivalDetailService);
        bool ValidDeleteObject(PurchaseReceivalDetail purchaseReceivalDetail, IPurchaseReceivalDetailService _purchaseReceivalDetailService);
        bool ValidConfirmObject(PurchaseReceivalDetail purchaseReceivalDetail, IItemService _itemService, IPurchaseOrderDetailService _purchaseOrderDetailService);
        bool ValidUnconfirmObject(PurchaseReceivalDetail purchaseReceivalDetail, IItemService _itemService);
        bool isValid(PurchaseReceivalDetail obj);
        string PrintError(PurchaseReceivalDetail obj);
    }
}
