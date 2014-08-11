using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DomainModel;
using Core.Interface.Validation;

namespace Core.Interface.Service
{
    public interface ISalesOrderService
    {
        ISalesOrderValidator GetValidator();
        IList<SalesOrder> GetAll();
        IList<SalesOrder> GetObjectsByContactId(int ContactId);
        SalesOrder GetObjectById(int Id);
        SalesOrder CreateObject(SalesOrder salesOrder, IContactService _contactService);
        SalesOrder UpdateObject(SalesOrder salesOrder, IContactService _contactService);
        SalesOrder ConfirmObject(SalesOrder salesOrder, ISalesOrderDetailService _salesOrderDetailService, IStockMutationService _stockMutationService, IItemService _itemService);
        SalesOrder UnconfirmObject(SalesOrder salesOrder, ISalesOrderDetailService _salesOrderDetailService, IStockMutationService _stockMutationService, IItemService _itemService, IDeliveryOrderDetailService _deliveryOrderDetailService);
        SalesOrder SoftDeleteObject(SalesOrder salesOrder, ISalesOrderDetailService _salesOrderDetailService);
        bool DeleteObject(int Id);
    }
}
