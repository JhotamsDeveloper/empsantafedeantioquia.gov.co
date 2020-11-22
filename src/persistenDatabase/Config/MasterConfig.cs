using Microsoft.EntityFrameworkCore.Metadata.Builders;
using model;
using System;
using System.Collections.Generic;
using System.Text;

namespace persistenDatabase.Config
{
    public class MasterConfig
    {
        public MasterConfig(EntityTypeBuilder<Master> entityBuilder)
        {
            entityBuilder.HasKey(x => x.Id);

            entityBuilder.Property(x => x.NameMaster)
                .IsRequired();

            entityBuilder.Property(x => x.Description)
                .IsRequired();

        }
    }
}
