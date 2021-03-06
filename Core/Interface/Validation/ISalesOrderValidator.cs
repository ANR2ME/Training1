﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DomainModel;
using Core.Interface.Service;

namespace Core.Interface.Validation
{
    public interface ISalesOrderValidator
    {
        SalesOrder VHasContact(SalesOrder salesOrder, IContactService _contactService);
        SalesOrder VIsValidSalesDate(SalesOrder salesOrder);
        SalesOrder VIsConfirmed(SalesOrder salesOrder);
        SalesOrder VIsNotConfirmed(SalesOrder salesOrder);
        SalesOrder VIsSalesOrderDetailsNotConfirmed(SalesOrder salesOrder, ISalesOrderDetailService _salesOrderDetailService);
        SalesOrder VHasSalesOrderDetails(SalesOrder salesOrder, ISalesOrderDetailService _salesOrderDetailService);
        SalesOrder VIsValidSalesOrderDetailsQuantity(SalesOrder salesOrder, ISalesOrderDetailService _salesOrderDetailService, IItemService _itemService);
        SalesOrder VConfirmObject(SalesOrder salesOrder, ISalesOrderDetailService _salesOrderDetailService, IItemService _itemService);
        SalesOrder VUnconfirmObject(SalesOrder salesOrder);
        SalesOrder VCreateObject(SalesOrder salesOrder, IContactService _contactService);
        SalesOrder VUpdateObject(SalesOrder salesOrder, IContactService _contactService);
        SalesOrder VDeleteObject(SalesOrder salesOrder, ISalesOrderDetailService _salesOrderDetailService);

        bool ValidCreateObject(SalesOrder salesOrder, IContactService _contactService);
        bool ValidUpdateObject(SalesOrder salesOrder, IContactService _contactService);
        bool ValidDeleteObject(SalesOrder salesOrder, ISalesOrderDetailService _salesOrderDetailService);
        bool ValidConfirmObject(SalesOrder salesOrder, ISalesOrderDetailService _salesOrderDetailService, IItemService _itemService);
        bool ValidUnconfirmObject(SalesOrder salesOrder);
        bool isValid(SalesOrder obj);
        string PrintError(SalesOrder obj);
    }
}
