using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DomainModel
{
    public partial class DeliveryOrderDetail
    {
        public int Id { get; set; }
        public int ItemId { get; set; }
        public int DeliveryOrderId { get; set; }
        public int SalesOrderDetailId { get; set; }
        public int Quantity { get; set; }
        public string Code { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsConfirmed { get; set; }
        public DateTime CreatedAt { get; set; }
        public Nullable<DateTime> UpdatedAt { get; set; }
        public Nullable<DateTime> DeletedAt { get; set; }
        public Nullable<DateTime> ConfirmationDate { get; set; }

        public Dictionary<string, string> Errors { get; set; }

        public virtual Item Item { get; set; }
        public virtual DeliveryOrder DeliveryOrder { get; set; }
        public virtual SalesOrderDetail SalesOrderDetail { get; set; }
    }
}
