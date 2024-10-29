using HSCodeDatabase.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HSCodeDatabase.DataAccess.EntityConfigurations;

public class SubCategoryConfiguration : IEntityTypeConfiguration<SubCategory>
{
    public void Configure(EntityTypeBuilder<SubCategory> builder)
    {
        builder.Property(rootCategory => rootCategory.HsCode).HasMaxLength(13);
        builder.Property(rootCategory => rootCategory.Description).HasMaxLength(1024);

        // Configure the relationship with RootCategory
        builder
            .HasOne(subCategory => subCategory.RootCategory)
            .WithMany(rootCategory => rootCategory.SubCategories)
            .HasForeignKey(subCategory => subCategory.RootCategoryId)
            .OnDelete(DeleteBehavior.Cascade); // This can remain cascade
    }
}

