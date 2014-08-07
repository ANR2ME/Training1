using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DomainModel;

namespace Data.Mapping
{
    public class DeliveryOrderMapping : EntityTypeConfiguration<DeliveryOrder>
    {
        public DeliveryOrderMapping()
        {
            HasKey(pr => pr.Id);
            HasMany(pr => pr.DeliveryOrderDetails)
                .WithRequired(prd => prd.DeliveryOrder)
                .HasForeignKey(prd => prd.DeliveryOrderId);
            Ignore(prd => prd.Errors);
        }
    }
}
