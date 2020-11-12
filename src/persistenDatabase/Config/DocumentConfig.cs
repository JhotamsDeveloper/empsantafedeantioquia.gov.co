using Microsoft.EntityFrameworkCore.Metadata.Builders;
using model;
using System;
using System.Collections.Generic;
using System.Text;

namespace persistenDatabase.Config
{
    public class DocumentConfig
    {

        public DocumentConfig(EntityTypeBuilder<Document> entityBuilder)
        {
            entityBuilder.HasKey(x => x.ID);

            entityBuilder.Property(x => x.Name)
            .IsRequired();


        }

    }
}
