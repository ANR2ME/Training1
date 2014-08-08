using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DomainModel;
using Core.Interface.Service;

namespace Core.Interface.Repository
{
    public interface ISalesOrderRepository : IRepository<SalesOrder>
    {
        IList<SalesOrder> GetAll();
        IList<SalesOrder> GetObjectsByContactId(int ContactId);
        SalesOrder GetObjectById(int Id);
        SalesOrder CreateObject(SalesOrder salesOrder);
        SalesOrder UpdateObject(SalesOrder salesOrder);
        SalesOrder SoftDeleteObject(SalesOrder salesOrder);
        SalesOrder ConfirmObject(SalesOrder salesOrder, ISalesOrderDetailService _salesOrderDetailService, IStockMutationService _stockMutationService, IItemService _itemService);
        SalesOrder UnconfirmObject(SalesOrder salesOrder, ISalesOrderDetailService _salesOrderDetailService, IStockMutationService _stockMutationService, IItemService _itemService, IDeliveryOrderDetailService _deliveryOrderDetailService);
        bool DeleteObject(int Id);
        string SetObjectCode(SalesOrder obj);
    }
}
