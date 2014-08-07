using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Constants
{
    public class Constant
    {
        public enum ItemCase
        {
            Ready = 1,
            PendingReceival = 2,
            PendingDelivery = 3,
        }

        public enum Status
        {
            Addition = 1,
            Deduction = 2,
        }

        public enum SourceDocumentType
        {
            StockAdjusment = 1,
            PurchaseOrder = 2,
            PurchaseReceival = 3,
            SalesOrder = 4,
            DeliveryOrder = 5,
        }

        public enum SourceDocumentDetailType
        {
            StockAdjusmentDetail = 1,
            PurchaseOrderDetail = 2,
            PurchaseReceivalDetail = 3,
            SalesOrderDetail = 4,
            DeliveryOrderDetail = 5,
        }
    }
}
