using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DomainModel;

namespace Data.Mapping
{
    public class PurchaseOrderDetailMapping : EntityTypeConfiguration<PurchaseOrderDetail>
    {
        public PurchaseOrderDetailMapping()
        {
            HasKey(pod => pod.Id);
            HasRequired(pod => pod.PurchaseOrder)
                .WithMany(po => po.PurchaseOrderDetails)
                .HasForeignKey(pod => pod.PurchaseOrderId);
            HasRequired(pod => pod.Item)
                .WithMany()
                .HasForeignKey(pod => pod.ItemId);
            HasMany(prd => prd.PurchaseReceivalDetails)
                .WithRequired(prd => prd.PurchaseOrderDetail)
                .HasForeignKey(prd => prd.PurchaseOrderDetailId);
            Ignore(pod => pod.Errors);
        }
    }
}
