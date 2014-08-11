using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DomainModel;
using Core.Interface.Service;
using Core.Interface.Validation;

namespace Validation.Validation
{
    public class StockMutationValidator : IStockMutationValidator
    {
        public StockMutation VIsValidItemCase(StockMutation stockMutation)
        {
            if (stockMutation.ItemCase != "Ready" && stockMutation.ItemCase != "PendingDelivery" && stockMutation.ItemCase != "PendingReceival")
            {
                stockMutation.Errors.Add("ItemCase", "Harus Ready/PendingDelivery/PendingReceival");
            }
            return stockMutation;
        }

        public StockMutation VIsValidStatus(StockMutation stockMutation)
        {
            if (stockMutation.Status != "Addition" && stockMutation.Status != "Deduction")
            {
                stockMutation.Errors.Add("Status", "Harus Addition/Deduction");
            }
            return stockMutation;
        }

        public StockMutation VHasItem(StockMutation stockMutation, IItemService _itemService)
        {
            Item i = _itemService.GetObjectById(stockMutation.ItemId);
            if (i == null)
            {
                stockMutation.Errors.Add("Item", "Harus ada");
            }
            return stockMutation;
        }

        public StockMutation VHasSourceDocument(StockMutation stockMutation)
        {
            if (stockMutation.SourceDocumentId <= 0)
            {
                stockMutation.Errors.Add("SourceDocument", "Harus ada");
            }
            return stockMutation;
        }

        public StockMutation VIsValidSourceDocumentType(StockMutation stockMutation)
        {
            if (stockMutation.SourceDocumentType != "StockAdjustment" && stockMutation.SourceDocumentType != "PurchaseOrder" && 
                stockMutation.SourceDocumentType != "PurchaseReceival" && stockMutation.SourceDocumentType != "SalesOrder" && 
                stockMutation.SourceDocumentType != "DeliveryOrder")
            {
                stockMutation.Errors.Add("SourceDocumentType", "Harus StockAdjustment/PurchaseOrder/PurchaseReceival/SalesOrder/DeliveryOrder");
            }
            return stockMutation;
        }

        public StockMutation VHasSourceDocumentDetail(StockMutation stockMutation)
        {
            if (stockMutation.SourceDocumentDetailId <= 0)
            {
                stockMutation.Errors.Add("SourceDocumentDetail", "Harus ada");
            }
            return stockMutation;
        }

        public StockMutation VIsValidSourceDocumentDetailType(StockMutation stockMutation)
        {
            if (stockMutation.SourceDocumentDetailType != "StockAdjustmentDetail" && stockMutation.SourceDocumentDetailType != "PurchaseOrderDetail" &&
                stockMutation.SourceDocumentDetailType != "PurchaseReceivalDetail" && stockMutation.SourceDocumentDetailType != "SalesOrderDetail" &&
                stockMutation.SourceDocumentDetailType != "DeliveryOrderDetail")
            {
                stockMutation.Errors.Add("SourceDocumentDetailType", "Harus StockAdjustmentDetail/PurchaseOrderDetail/PurchaseReceivalDetail/SalesOrderDetail/DeliveryOrderDetail");
            }
            return stockMutation;
        }

        public StockMutation VIsPositiveQuantity(StockMutation stockMutation)
        {
            if (stockMutation.Quantity <= 0)
            {
                stockMutation.Errors.Add("Quantity", "Harus lebih besar dari 0");
            }
            return stockMutation;
        }

        public StockMutation VCreateObject(StockMutation stockMutation, IItemService _itemService)
        {
            VIsValidItemCase(stockMutation);
            VIsValidStatus(stockMutation);
            VHasItem(stockMutation, _itemService);
            VHasSourceDocument(stockMutation);
            VHasSourceDocumentDetail(stockMutation);
            VIsValidSourceDocumentType(stockMutation);
            VIsValidSourceDocumentDetailType(stockMutation);
            VIsPositiveQuantity(stockMutation);
            return stockMutation;
        }

        public StockMutation VStockMutateObject(StockMutation stockMutation)
        {

            return stockMutation;
        }

        public StockMutation VReverseStockMutateObject(StockMutation stockMutation)
        {

            return stockMutation;
        }

        public StockMutation VDeleteObject(StockMutation stockMutation)
        {

            return stockMutation;
        }

        public bool ValidCreateObject(StockMutation stockMutation, IItemService _itemService)
        {
            VCreateObject(stockMutation, _itemService);
            return isValid(stockMutation);
        }

        public bool ValidStockMutateObject(StockMutation stockMutation)
        {
            stockMutation.Errors.Clear();
            VStockMutateObject(stockMutation);
            return isValid(stockMutation);
        }

        public bool ValidReverseStockMutateObject(StockMutation stockMutation)
        {
            stockMutation.Errors.Clear();
            VReverseStockMutateObject(stockMutation);
            return isValid(stockMutation);
        }

        public bool ValidDeleteObject(StockMutation stockMutation)
        {
            stockMutation.Errors.Clear();
            VDeleteObject(stockMutation);
            return isValid(stockMutation);
        }

        public bool isValid(StockMutation obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }

        public string PrintError(StockMutation obj)
        {
            string erroroutput = "";
            KeyValuePair<string, string> first = obj.Errors.ElementAt(0);
            erroroutput += first.Key + "," + first.Value;
            foreach (KeyValuePair<string, string> pair in obj.Errors.Skip(1))
            {
                erroroutput += Environment.NewLine;
                erroroutput += pair.Key + "," + pair.Value;
            }
            return erroroutput;
        }
    }
}
