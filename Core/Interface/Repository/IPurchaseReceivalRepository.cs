using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DomainModel;
using Core.Interface.Service;

namespace Core.Interface.Repository
{
    public interface IPurchaseReceivalRepository : IRepository<PurchaseReceival>
    {
        IList<PurchaseReceival> GetAll();
        PurchaseReceival GetObjectById(int Id);
        PurchaseReceival CreateObject(PurchaseReceival purchaseReceival);
        PurchaseReceival UpdateObject(PurchaseReceival purchaseReceival);
        PurchaseReceival SoftDeleteObject(PurchaseReceival purchaseReceival);
        PurchaseReceival ConfirmObject(PurchaseReceival purchaseReceival, IPurchaseReceivalDetailService _purchaseReceivalDetailService, IStockMutationService _stockMutationService, IItemService _itemService, IPurchaseOrderDetailService _purchaseOrderDetailService);
        PurchaseReceival UnconfirmObject(PurchaseReceival purchaseReceival, IPurchaseReceivalDetailService _purchaseReceivalDetailService, IStockMutationService _stockMutationService, IItemService _itemService);
        bool DeleteObject(int Id);
        string SetObjectCode(PurchaseReceival obj);
    }
}
