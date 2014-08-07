﻿using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DomainModel;

namespace Data.Mapping
{
    public class PurchaseOrderMapping : EntityTypeConfiguration<PurchaseOrder>
    {
        public PurchaseOrderMapping()
        {
            HasKey(po => po.Id);
            HasMany(po => po.PurchaseOrderDetails)
                .WithRequired(pod => pod.PurchaseOrder)
                .HasForeignKey(pod => pod.PurchaseOrderId);
            Ignore(po => po.Errors);
        }
    }
}
