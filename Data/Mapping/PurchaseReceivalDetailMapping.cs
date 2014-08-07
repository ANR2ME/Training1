using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DomainModel;

namespace Data.Mapping
{
    public class PurchaseReceivalDetailMapping : EntityTypeConfiguration<PurchaseReceivalDetail>
    {
        public PurchaseReceivalDetailMapping()
        {
            HasKey(prd => prd.Id);
            HasRequired(prd => prd.PurchaseReceival)
                .WithMany(pr => pr.PurchaseReceivalDetails)
                .HasForeignKey(prd => prd.PurchaseReceivalId);
            HasRequired(prd => prd.Item)
                .WithMany()
                .HasForeignKey(prd => prd.ItemId)
                .WillCascadeOnDelete(false);
            HasRequired(prd => prd.PurchaseOrderDetail)
                .WithMany()
                .HasForeignKey(prd => prd.PurchaseOrderDetailId)
                .WillCascadeOnDelete(false);
            Ignore(prd => prd.Errors);
        }
    }
}
