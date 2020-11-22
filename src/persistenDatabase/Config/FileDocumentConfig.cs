using Microsoft.EntityFrameworkCore.Metadata.Builders;
using model;
using System;
using System.Collections.Generic;
using System.Text;

namespace persistenDatabase.Config
{
    public class FileDocumentConfig
    {
        public FileDocumentConfig(EntityTypeBuilder<FileDocument> entityBuilder)
        {
            entityBuilder.HasKey(x => x.ID);

            entityBuilder.Property(x => x.NameFile)
                .IsRequired();

            entityBuilder.Property(x => x.RouteFile)
            .IsRequired();

            //One to many -> Uno a muchos
            entityBuilder.HasOne(x => x.Document)
                .WithMany(x => x.FileDocument)
                .HasForeignKey(x => x.DocumentoId)
                .OnDelete(Microsoft.EntityFrameworkCore.DeleteBehavior.Cascade);

            entityBuilder.HasOne(x => x.Masters)
                .WithMany(x => x.FileDocument)
                .HasForeignKey(x => x.MasterId)
                .OnDelete(Microsoft.EntityFrameworkCore.DeleteBehavior.Cascade);
        }
    }
}
