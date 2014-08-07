using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DomainModel;

namespace Core.Interface.Validation
{
    public interface IStockMutationValidator
    {
        StockMutation VIsValidItemCase(StockMutation stockMutation);
        StockMutation VIsValidStatus(StockMutation stockMutation);
        StockMutation VHasItem(StockMutation stockMutation);
        StockMutation VHasSourceDocument(StockMutation stockMutation);
        StockMutation VHasSourceDocumentDetail(StockMutation stockMutation);
        StockMutation VIsValidSourceDocumentType(StockMutation stockMutation);
        StockMutation VIsValidSourceDocumentDetailType(StockMutation stockMutation);
        StockMutation VIsValidQuantity(StockMutation stockMutation);

        StockMutation VCreateObject(StockMutation stockMutation);
        StockMutation VStockMutateObject(StockMutation stockMutation);
        StockMutation VReverseStockMutateObject(StockMutation stockMutation);
        StockMutation VDeleteObject(StockMutation stockMutation);
        bool ValidCreateObject(StockMutation stockMutation);
        bool ValidStockMutateObject(StockMutation stockMutation);
        bool ValidReverseStockMutateObject(StockMutation stockMutation);
        bool ValidDeleteObject(StockMutation stockMutation);
        bool isValid(StockMutation stockMutation);
        string PrintError(StockMutation stockMutation);
    }
}
