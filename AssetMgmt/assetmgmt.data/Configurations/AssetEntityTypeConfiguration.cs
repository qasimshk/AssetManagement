using assetmgmt.data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace assetmgmt.data.Configurations
{
    public class AssetEntityTypeConfiguration : IEntityTypeConfiguration<Asset>
    {
        public void Configure(EntityTypeBuilder<Asset> builder)
        {
            builder.ToTable(nameof(Asset));

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
                .HasMaxLength(40)
                .IsRequired();

            builder.Property(x => x.Symbol)
                .HasMaxLength(10)
                .IsRequired();

            builder.Property(x => x.ISIN)
                .HasMaxLength(20)
                .IsRequired();

            builder.HasMany(asset => asset.Sources)
                .WithOne(src => src.Asset)
                .HasForeignKey(src => src.AssetId);

            builder.HasIndex(x => x.Symbol);

            builder.HasIndex(x => x.ISIN);
        }
    }
}
