using Microsoft.EntityFrameworkCore.Metadata.Builders;
using model;
using System;
using System.Collections.Generic;
using System.Text;

namespace persistenDatabase.Config
{
    class BiddingParticipantConfig
    {
        public BiddingParticipantConfig(EntityTypeBuilder<BiddingParticipant> entityBuilder)
        {
            entityBuilder.HasKey(x => x.Id);

            entityBuilder.Property(x => x.Name)
                .IsRequired();

            //One to many -> Uno a muchos
            entityBuilder.HasOne(x => x.Master)
                .WithMany(x => x.BiddingParticipants)
                .HasForeignKey(x => x.MasterId);
        }
    }
}
