using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DomainModel;

namespace Data.Mapping
{
    public class ContactMapping : EntityTypeConfiguration<Contact>
    {
        public ContactMapping()
        {
            HasKey(i => i.Id);
            HasMany(i => i.PurchaseOrders)
                .WithRequired(po => po.Contact)
                .HasForeignKey(po => po.ContactId);
            HasMany(i => i.SalesOrders)
                .WithRequired(so => so.Contact)
                .HasForeignKey(so => so.ContactId);
            Ignore(i => i.Errors);
        }
    }
}
