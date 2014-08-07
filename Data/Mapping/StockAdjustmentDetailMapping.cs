using System.Data.Entity.ModelConfiguration;
using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Mapping
{
    public class StockAdjustmenDetailMapping : EntityTypeConfiguration<StockAdjustmentDetail>
    {
        public StockAdjustmenDetailMapping()
        {
            HasKey(sad => sad.Id);
            HasRequired(sad => sad.StockAdjustment)
                .WithMany(sa => sa.StockAdjustmentDetails)
                .HasForeignKey(sad => sad.StockAdjustmentId);
            HasRequired(sad => sad.Item)
                .WithMany()
                .HasForeignKey(sad => sad.ItemId);
            Ignore(sad => sad.Errors);
        }
    }
}
