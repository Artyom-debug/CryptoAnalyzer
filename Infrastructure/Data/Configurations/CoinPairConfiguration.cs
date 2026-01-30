using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class CoinPairConfiguration : IEntityTypeConfiguration<CoinPair>
{
    public void Configure(EntityTypeBuilder<CoinPair> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Symbol)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasIndex(x => x.Symbol)
            .IsUnique();
    }
}
