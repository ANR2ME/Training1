using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DomainModel;

namespace Data.Mapping
{
    public class SalesOrderMapping : EntityTypeConfiguration<SalesOrder>
    {
        public SalesOrderMapping()
        {
            HasKey(po => po.Id);
            HasMany(po => po.SalesOrderDetails)
                .WithRequired(pod => pod.SalesOrder)
                .HasForeignKey(pod => pod.SalesOrderId);
            HasRequired(x => x.Contact)
                .WithMany(i => i.SalesOrders)
                .HasForeignKey(z => z.ContactId)
                .WillCascadeOnDelete(false);
            Ignore(po => po.Errors);
        }
    }
}
