using assetmgmt.data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace assetmgmt.data.Configurations
{
    public class SourceEntityTypeConfiguration : IEntityTypeConfiguration<Source>
    {
        public void Configure(EntityTypeBuilder<Source> builder)
        {
            builder.ToTable(nameof(Source));

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
                .HasMaxLength(40)
                .IsRequired();

            builder.Property(pro => pro.Price)
                .HasColumnType("money")
                .HasConversion<decimal>()
                .IsRequired();

            builder.Property(x => x.AssetId)
                .IsRequired();

            builder.HasIndex(x => x.Name);
        }
    }
}
