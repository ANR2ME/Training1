using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DomainModel;
using Core.Interface.Service;

namespace Core.Interface.Repository
{
    public interface ISalesOrderDetailRepository : IRepository<SalesOrderDetail>
    {
        IList<SalesOrderDetail> GetAll();
        IList<SalesOrderDetail> GetObjectsByItemId(int ItemId);
        IList<SalesOrderDetail> GetObjectsBySalesOrderId(int SalesOrderId);
        IList<SalesOrderDetail> GetConfirmedObjectsBySalesOrderId(int SalesOrderId);

        SalesOrderDetail GetObjectById(int Id);
        SalesOrderDetail CreateObject(SalesOrderDetail salesOrderDetail, string ParentCode);
        SalesOrderDetail UpdateObject(SalesOrderDetail salesOrderDetail);
        SalesOrderDetail SoftDeleteObject(SalesOrderDetail salesOrderDetail);
        SalesOrderDetail ConfirmObject(SalesOrderDetail salesOrderDetail, IStockMutationService _stockMutationService, IItemService _itemService);
        SalesOrderDetail UnconfirmObject(SalesOrderDetail salesOrderDetail, IStockMutationService _stockMutationService, IItemService _itemService);

        bool DeleteObject(int Id);
        string SetObjectCode(SalesOrderDetail obj, string ParentCode);
    }
}
