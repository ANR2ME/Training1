using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DomainModel;
using Core.Interface.Service;

namespace Core.Interface.Validation
{
    public interface IPurchaseOrderValidator
    {
        PurchaseOrder VHasCustomer(PurchaseOrder purchaseOrder);
        PurchaseOrder VIsValidPurchaseDate(PurchaseOrder purchaseOrder);
        PurchaseOrder VIsConfirmed(PurchaseOrder purchaseOrder);
        PurchaseOrder VIsNotConfirmed(PurchaseOrder purchaseOrder);
        PurchaseOrder VIsPurchaseOrderDetailsNotConfirmed(PurchaseOrder purchaseOrder, IPurchaseOrderDetailService _purchaseOrderDetailService);
        PurchaseOrder VHasPurchaseOrderDetails(PurchaseOrder purchaseOrder, IPurchaseOrderDetailService _purchaseOrderDetailService);
        //PurchaseOrder VIsValidPurchaseOrderDetailsQuantity(PurchaseOrder purchaseOrder, IPurchaseOrderDetailService _purchaseOrderDetailService, IItemService _itemService);
        PurchaseOrder VConfirmObject(PurchaseOrder purchaseOrder, IPurchaseOrderDetailService _purchaseOrderDetailService, IItemService _itemService);
        PurchaseOrder VUnconfirmObject(PurchaseOrder purchaseOrder);
        PurchaseOrder VCreateObject(PurchaseOrder purchaseOrder);
        PurchaseOrder VUpdateObject(PurchaseOrder purchaseOrder);
        PurchaseOrder VDeleteObject(PurchaseOrder purchaseOrder, IPurchaseOrderDetailService _purchaseOrderDetailService);

        bool ValidCreateObject(PurchaseOrder purchaseOrder);
        bool ValidUpdateObject(PurchaseOrder purchaseOrder);
        bool ValidDeleteObject(PurchaseOrder purchaseOrder, IPurchaseOrderDetailService _purchaseOrderDetailService);
        bool ValidConfirmObject(PurchaseOrder purchaseOrder, IPurchaseOrderDetailService _purchaseOrderDetailService, IItemService _itemService);
        bool ValidUnconfirmObject(PurchaseOrder purchaseOrder);
        bool isValid(PurchaseOrder obj);
        string PrintError(PurchaseOrder obj);
    }
}
