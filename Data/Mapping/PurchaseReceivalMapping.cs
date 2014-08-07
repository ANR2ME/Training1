﻿using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DomainModel;

namespace Data.Mapping
{
    public class PurchaseReceivalMapping : EntityTypeConfiguration<PurchaseReceival>
    {
        public PurchaseReceivalMapping()
        {
            HasKey(pr => pr.Id);
            HasMany(pr => pr.PurchaseReceivalDetails)
                .WithRequired(prd => prd.PurchaseReceival)
                .HasForeignKey(prd => prd.PurchaseReceivalId);
            Ignore(prd => prd.Errors);
        }
    }
}
