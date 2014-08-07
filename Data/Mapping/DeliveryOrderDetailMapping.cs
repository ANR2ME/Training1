using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DomainModel;

namespace Data.Mapping
{
    public class DeliveryOrderDetailMapping : EntityTypeConfiguration<DeliveryOrderDetail>
    {
        public DeliveryOrderDetailMapping()
        {
            HasKey(prd => prd.Id);
            HasRequired(prd => prd.DeliveryOrder)
                .WithMany(pr => pr.DeliveryOrderDetails)
                .HasForeignKey(prd => prd.DeliveryOrderId);
            HasRequired(prd => prd.Item)
                .WithMany()
                .HasForeignKey(prd => prd.ItemId)
                .WillCascadeOnDelete(false);
            HasRequired(prd => prd.SalesOrderDetail)
                .WithMany()
                .HasForeignKey(prd => prd.SalesOrderDetailId)
                .WillCascadeOnDelete(false);
            Ignore(prd => prd.Errors);
        }
    }
}
