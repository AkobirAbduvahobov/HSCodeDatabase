using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HSCodeDatabase.DataAccess.Entities;

namespace HSCodeDatabase.DataAccess.EntityConfigurations;

public class RootConfiguration : IEntityTypeConfiguration<Root>
{
    public void Configure(EntityTypeBuilder<Root> builder)
    {
        builder.Property(rootCategory => rootCategory.HsCode).HasMaxLength(13);
        builder.Property(rootCategory => rootCategory.Description).HasMaxLength(1024);
    }
}
