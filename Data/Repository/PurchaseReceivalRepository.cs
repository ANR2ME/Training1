using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DomainModel;
using Core.Interface.Repository;
using Core.Interface.Service;
using Data.Context;

namespace Data.Repository
{
    public class PurchaseReceivalRepository : EfRepository<PurchaseReceival>, IPurchaseReceivalRepository
    {
        private StockControlEntities entities;
        public PurchaseReceivalRepository()
        {
            entities = new StockControlEntities();
        }

        public IList<PurchaseReceival> GetAll()
        {
            return FindAll().ToList();
        }

        public PurchaseReceival GetObjectById(int Id)
        {
            PurchaseReceival pr = Find(x => x.Id == Id && !x.IsDeleted);
            if (pr != null) { pr.Errors = new Dictionary<string, string>(); }
            return pr;
        }

        public PurchaseReceival CreateObject(PurchaseReceival purchaseReceival)
        {
            purchaseReceival.CreatedAt = DateTime.Now;
            purchaseReceival.IsDeleted = false;
            purchaseReceival.IsConfirmed = false;
            purchaseReceival.Code = SetObjectCode(purchaseReceival);
            return Create(purchaseReceival);
        }

        public PurchaseReceival UpdateObject(PurchaseReceival purchaseReceival)
        {
            purchaseReceival.UpdatedAt = DateTime.Now;
            Update(purchaseReceival);
            return purchaseReceival;
        }

        public PurchaseReceival SoftDeleteObject(PurchaseReceival purchaseReceival)
        {
            purchaseReceival.IsDeleted = true;
            purchaseReceival.DeletedAt = DateTime.Now;
            Update(purchaseReceival);
            return purchaseReceival;
        }

        public PurchaseReceival ConfirmObject(PurchaseReceival purchaseReceival, IPurchaseReceivalDetailService _purchaseReceivalDetailService, IStockMutationService _stockMutationService, IItemService _itemService, IPurchaseOrderDetailService _purchaseOrderDetailService)
        {
            IList<PurchaseReceivalDetail> purchaseReceivalDetails = _purchaseReceivalDetailService.GetObjectsByPurchaseReceivalId(purchaseReceival.Id);
            int Confirmed = 0;
            foreach (var prd in purchaseReceivalDetails)
            {
                if (prd.IsConfirmed) Confirmed--; // Already Confirmed
                _purchaseReceivalDetailService.ConfirmObject(prd, _stockMutationService, _itemService, _purchaseOrderDetailService);
                if (prd.IsConfirmed) Confirmed++; // Newly Confirmed
            }
            if (Confirmed > 0)
            {
                purchaseReceival.IsConfirmed = true;
                purchaseReceival.ConfirmationDate = DateTime.Now;
                Update(purchaseReceival);
            }
            return purchaseReceival;
        }

        public PurchaseReceival UnconfirmObject(PurchaseReceival purchaseReceival, IPurchaseReceivalDetailService _purchaseReceivalDetailService, IStockMutationService _stockMutationService, IItemService _itemService)
        {
            purchaseReceival.IsConfirmed = false;
            purchaseReceival.ConfirmationDate = null;
            purchaseReceival.UpdatedAt = DateTime.Now;
            Update(purchaseReceival);
            IList<PurchaseReceivalDetail> purchaseReceivalDetails = _purchaseReceivalDetailService.GetObjectsByPurchaseReceivalId(purchaseReceival.Id);
            foreach (var prd in purchaseReceivalDetails)
            {
                _purchaseReceivalDetailService.UnconfirmObject(prd, _stockMutationService, _itemService);
            }
            return purchaseReceival;
        }

        public bool DeleteObject(int Id)
        {
            PurchaseReceival pr = Find(x => x.Id == Id);
            return (Delete(pr) == 1) ? true : false;
        }

        public string SetObjectCode(PurchaseReceival obj)
        {
            // Code = #{currentyear}/#{totalnumber + 1}
            int totalobject = FindAll(x => x.CreatedAt.Year == DateTime.Now.Year).Count() + 1;
            string Code = DateTime.Now.Year.ToString() + "/" + totalobject;
            return Code;
        }
    }
}
