using Microsoft.EntityFrameworkCore.Metadata.Builders;
using model;
using System;
using System.Collections.Generic;
using System.Text;

namespace persistenDatabase.Config
{
    public class EmployeeConfig
    {
        public EmployeeConfig(EntityTypeBuilder<Employee> entityBuilder)
        {
            entityBuilder.HasKey(x => x.EmployeeId);

        }
    }
}
