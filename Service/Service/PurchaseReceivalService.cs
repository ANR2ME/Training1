using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DomainModel;
using Core.Interface.Repository;
using Core.Interface.Service;
using Core.Interface.Validation;

namespace Service.Service
{
    public class PurchaseReceivalService : IPurchaseReceivalService
    {
        private IPurchaseReceivalRepository _repository;
        private IPurchaseReceivalValidator _validator;
        public PurchaseReceivalService(IPurchaseReceivalRepository _purchaseReceivalRepository, IPurchaseReceivalValidator _purchaseReceivalValidator)
        {
            _repository = _purchaseReceivalRepository;
            _validator = _purchaseReceivalValidator;
        }

        public IPurchaseReceivalValidator GetValidator()
        {
            return _validator;
        }

        public IList<PurchaseReceival> GetAll()
        {
            return _repository.GetAll();
        }

        public PurchaseReceival GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public PurchaseReceival CreateObject(PurchaseReceival purchaseReceival, IPurchaseOrderService _purchaseOrderService, IContactService _contactService)
        {
            purchaseReceival.Errors = new Dictionary<String, String>();
            return (purchaseReceival = _validator.ValidCreateObject(purchaseReceival, _purchaseOrderService, _contactService) ? _repository.CreateObject(purchaseReceival) : purchaseReceival);
        }

        public PurchaseReceival UpdateObject(PurchaseReceival purchaseReceival, IPurchaseOrderService _purchaseOrderService, IContactService _contactService)
        {
            return (purchaseReceival = _validator.ValidUpdateObject(purchaseReceival, _purchaseOrderService, _contactService) ? _repository.UpdateObject(purchaseReceival) : purchaseReceival);
        }

        public PurchaseReceival ConfirmObject(PurchaseReceival purchaseReceival, IPurchaseReceivalDetailService _purchaseReceivalDetailService, IStockMutationService _stockMutationService, IItemService _itemService, IPurchaseOrderDetailService _purchaseOrderDetailService)
        {

            return (purchaseReceival = _validator.ValidConfirmObject(purchaseReceival, _purchaseReceivalDetailService, _itemService) ? _repository.ConfirmObject(purchaseReceival, _purchaseReceivalDetailService, _stockMutationService, _itemService, _purchaseOrderDetailService) : purchaseReceival);
        }

        public PurchaseReceival UnconfirmObject(PurchaseReceival purchaseReceival, IPurchaseReceivalDetailService _purchaseReceivalDetailService, IStockMutationService _stockMutationService, IItemService _itemService)
        {

            return (purchaseReceival = _validator.ValidUnconfirmObject(purchaseReceival) ? _repository.UnconfirmObject(purchaseReceival, _purchaseReceivalDetailService, _stockMutationService, _itemService) : purchaseReceival);
        }

        public PurchaseReceival SoftDeleteObject(PurchaseReceival purchaseReceival, IPurchaseReceivalDetailService _purchaseReceivalDetailService)
        {
            return (purchaseReceival = _validator.ValidDeleteObject(purchaseReceival, _purchaseReceivalDetailService) ? _repository.SoftDeleteObject(purchaseReceival) : purchaseReceival);
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }
    }
}
