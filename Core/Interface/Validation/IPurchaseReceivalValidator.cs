using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DomainModel;
using Core.Interface.Service;

namespace Core.Interface.Validation
{
    public interface IPurchaseReceivalValidator
    {
        PurchaseReceival VHasPurchaseOrder(PurchaseReceival purchaseReceival, IPurchaseOrderService _purchaseOrderService);
        PurchaseReceival VHasContact(PurchaseReceival purchaseReceival, IPurchaseOrderService _purchaseOrderService, IContactService _contactService);
        PurchaseReceival VIsValidReceivalDate(PurchaseReceival purchaseReceival);
        PurchaseReceival VIsConfirmed(PurchaseReceival purchaseReceival);
        PurchaseReceival VIsNotConfirmed(PurchaseReceival purchaseReceival);
        PurchaseReceival VIsPurchaseReceivalDetailsNotConfirmed(PurchaseReceival purchaseReceival, IPurchaseReceivalDetailService _purchaseReceivalDetailService);
        PurchaseReceival VHasPurchaseReceivalDetails(PurchaseReceival purchaseReceival, IPurchaseReceivalDetailService _purchaseReceivalDetailService);
        PurchaseReceival VConfirmObject(PurchaseReceival purchaseReceival, IPurchaseReceivalDetailService _purchaseReceivalDetailService, IItemService _itemService);
        PurchaseReceival VUnconfirmObject(PurchaseReceival purchaseReceival);
        PurchaseReceival VCreateObject(PurchaseReceival purchaseReceival, IPurchaseOrderService _purchaseOrderService, IContactService _contactService);
        PurchaseReceival VUpdateObject(PurchaseReceival purchaseReceival, IPurchaseOrderService _purchaseOrderService, IContactService _contactService);
        PurchaseReceival VDeleteObject(PurchaseReceival purchaseReceival, IPurchaseReceivalDetailService _purchaseReceivalDetailService);

        bool ValidCreateObject(PurchaseReceival purchaseReceival, IPurchaseOrderService _purchaseOrderService, IContactService _contactService);
        bool ValidUpdateObject(PurchaseReceival purchaseReceival, IPurchaseOrderService _purchaseOrderService, IContactService _contactService);
        bool ValidDeleteObject(PurchaseReceival purchaseReceival, IPurchaseReceivalDetailService _purchaseReceivalDetailService);
        bool ValidConfirmObject(PurchaseReceival purchaseReceival, IPurchaseReceivalDetailService _purchaseReceivalDetailService, IItemService _itemService);
        bool ValidUnconfirmObject(PurchaseReceival purchaseReceival);
        bool isValid(PurchaseReceival obj);
        string PrintError(PurchaseReceival obj);
    }
}
