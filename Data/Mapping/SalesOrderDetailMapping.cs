using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DomainModel;

namespace Data.Mapping
{
    public class SalesOrderDetailMapping : EntityTypeConfiguration<SalesOrderDetail>
    {
        public SalesOrderDetailMapping()
        {
            HasKey(pod => pod.Id);
            HasRequired(pod => pod.SalesOrder)
                .WithMany(po => po.SalesOrderDetails)
                .HasForeignKey(pod => pod.SalesOrderId);
            HasRequired(pod => pod.Item)
                .WithMany()
                .HasForeignKey(pod => pod.ItemId);
            HasMany(prd => prd.DeliveryOrderDetails)
                .WithRequired(prd => prd.SalesOrderDetail)
                .HasForeignKey(prd => prd.SalesOrderDetailId);
            Ignore(pod => pod.Errors);
        }
    }
}
