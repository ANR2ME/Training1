using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DomainModel;
using Core.Interface.Service;

namespace Core.Interface.Repository
{
    public interface IPurchaseReceivalDetailRepository : IRepository<PurchaseReceivalDetail>
    {
        IList<PurchaseReceivalDetail> GetAll();
        IList<PurchaseReceivalDetail> GetObjectsByItemId(int ItemId);
        IList<PurchaseReceivalDetail> GetObjectsByPurchaseReceivalId(int purchaseReceivalId);
        IList<PurchaseReceivalDetail> GetConfirmedObjectsByPurchaseReceivalId(int purchaseReceivalId);
        IList<PurchaseReceivalDetail> GetObjectsByPurchaseOrderDetailId(int PurchaseOrderDetailId);
        PurchaseReceivalDetail GetObjectById(int Id);
        PurchaseReceivalDetail CreateObject(PurchaseReceivalDetail purchaseReceivalDetail, string ParentCode);
        PurchaseReceivalDetail UpdateObject(PurchaseReceivalDetail purchaseReceivalDetail);
        PurchaseReceivalDetail SoftDeleteObject(PurchaseReceivalDetail purchaseReceivalDetail);
        PurchaseReceivalDetail ConfirmObject(PurchaseReceivalDetail purchaseReceivalDetail, IStockMutationService _stockMutationService, IItemService _itemService);
        PurchaseReceivalDetail UnconfirmObject(PurchaseReceivalDetail purchaseReceivalDetail, IStockMutationService _stockMutationService, IItemService _itemService);
        bool DeleteObject(int Id);
        string SetObjectCode(PurchaseReceivalDetail obj, string ParentCode);
    }
}
