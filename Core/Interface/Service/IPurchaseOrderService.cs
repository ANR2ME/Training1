using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DomainModel;
using Core.Interface.Validation;

namespace Core.Interface.Service
{
    public interface IPurchaseOrderService
    {
        IPurchaseOrderValidator GetValidator();
        IList<PurchaseOrder> GetAll();
        IList<PurchaseOrder> GetObjectsByContactId(int ContactId);
        PurchaseOrder GetObjectById(int Id);
        PurchaseOrder CreateObject(PurchaseOrder purchaseOrder, IContactService _contactService);
        PurchaseOrder UpdateObject(PurchaseOrder purchaseOrder, IContactService _contactService);
        PurchaseOrder ConfirmObject(PurchaseOrder purchaseOrder, IPurchaseOrderDetailService _purchaseOrderDetailService, IStockMutationService _stockMutationService, IItemService _itemService);
        PurchaseOrder UnconfirmObject(PurchaseOrder purchaseOrder, IPurchaseOrderDetailService _purchaseOrderDetailService, IStockMutationService _stockMutationService, IItemService _itemService, IPurchaseReceivalDetailService _purchaseReceivalDetailService);
        PurchaseOrder SoftDeleteObject(PurchaseOrder purchaseOrder, IPurchaseOrderDetailService _purchaseOrderDetailService);
        bool DeleteObject(int Id);
    }
}
