using Microsoft.EntityFrameworkCore.Metadata.Builders;
using model;
using System;
using System.Collections.Generic;
using System.Text;

namespace persistenDatabase.Config
{
    public class PQRSDConfig
    {
        public PQRSDConfig(EntityTypeBuilder<PQRSD> entityBuilder)
        {
            entityBuilder.HasKey(x => x.PQRSDID);

        }
    }
}
