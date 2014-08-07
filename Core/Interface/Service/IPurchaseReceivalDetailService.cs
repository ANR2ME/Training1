using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DomainModel;
using Core.Interface.Validation;

namespace Core.Interface.Service
{
    public interface IPurchaseReceivalDetailService
    {
        IPurchaseReceivalDetailValidator GetValidator();
        IList<PurchaseReceivalDetail> GetAll();
        IList<PurchaseReceivalDetail> GetObjectsByPurchaseReceivalId(int PurchaseReceivalId);
        IList<PurchaseReceivalDetail> GetConfirmedObjectsByPurchaseReceivalId(int PurchaseReceivalId);
        IList<PurchaseReceivalDetail> GetObjectsByPurchaseOrderDetailId(int PurchaseOrderDetailId);
        PurchaseReceivalDetail GetObjectById(int Id);
        PurchaseReceivalDetail CreateObject(PurchaseReceivalDetail purchaseReceivalDetail, IPurchaseReceivalService _purchaseReceivalService, IPurchaseOrderDetailService _purchaseOrderDetailService);
        PurchaseReceivalDetail UpdateObject(PurchaseReceivalDetail purchaseReceivalDetail);
        PurchaseReceivalDetail SoftDeleteObject(PurchaseReceivalDetail purchaseReceivalDetail);
        PurchaseReceivalDetail ConfirmObject(PurchaseReceivalDetail purchaseReceivalDetail, IStockMutationService _stockMutationService, IItemService _itemService, IPurchaseOrderDetailService _purchaseOrderDetailService);
        PurchaseReceivalDetail UnconfirmObject(PurchaseReceivalDetail purchaseReceivalDetail, IStockMutationService _stockMutationService, IItemService _itemService);
        bool DeleteObject(int Id);
    }
}
