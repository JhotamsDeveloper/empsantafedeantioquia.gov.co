using Microsoft.EntityFrameworkCore.Metadata.Builders;
using model;
using System;
using System.Collections.Generic;
using System.Text;

namespace persistenDatabase.Config
{
    public class CategoryConfig
    {
        public CategoryConfig(EntityTypeBuilder<Category> entityBuilder)
        {
            entityBuilder.HasKey(x => x.Id);

            entityBuilder.Property(x => x.NameCategory)
                .IsRequired();

            entityBuilder.Property(x => x.Description)
                .IsRequired();
        }
    }
}
