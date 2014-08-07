using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.DomainModel;
using Core.Interface.Repository;
using Core.Interface.Service;

namespace Data.Repository
{
    public interface IPurchaseOrderDetailRepository : IRepository<PurchaseOrderDetail>
    {
        IList<PurchaseOrderDetail> GetAll();
        IList<PurchaseOrderDetail> GetObjectsByItemId(int ItemId);
        IList<PurchaseOrderDetail> GetObjectsByPurchaseOrderId(int PurchaseOrderId);
        IList<PurchaseOrderDetail> GetConfirmedObjectsByPurchaseOrderId(int PurchaseOrderId);

        PurchaseOrderDetail GetObjectById(int Id);
        PurchaseOrderDetail CreateObject(PurchaseOrderDetail purchaseOrderDetail, string ParentCode);
        PurchaseOrderDetail UpdateObject(PurchaseOrderDetail purchaseOrderDetail);
        PurchaseOrderDetail SoftDeleteObject(PurchaseOrderDetail purchaseOrderDetail);
        PurchaseOrderDetail ConfirmObject(PurchaseOrderDetail purchaseOrderDetail, IStockMutationService _stockMutationService, IItemService _itemService);
        PurchaseOrderDetail UnconfirmObject(PurchaseOrderDetail purchaseOrderDetail, IStockMutationService _stockMutationService, IItemService _itemService);

        bool DeleteObject(int Id);
        string SetObjectCode(PurchaseOrderDetail obj, string ParentCode);
    }
}
