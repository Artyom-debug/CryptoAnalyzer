using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities;

namespace Infrastructure.Data.Configurations;

public class AnalyticsReportConfiguration : IEntityTypeConfiguration<AnalyticsReport>
{
    public void Configure(EntityTypeBuilder<AnalyticsReport> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.CoinPairId).IsRequired();

        builder.HasOne(x => x.CoinPair)
            .WithMany()
            .HasForeignKey(x => x.CoinPairId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(x => x.CreatedAt)
            .IsRequired();

        builder.Property(x => x.CandleOpen)
            .IsRequired();

        builder.Property(x => x.CandleClose)
            .IsRequired();

        builder.OwnsOne(x => x.Probability, pr =>
        {
            pr.Property(p => p.ProbabilityUp)
                .HasPrecision(9, 6)
                .IsRequired();

            pr.Property(p => p.ProbabilityDown)
                .HasPrecision(9, 6)
                .IsRequired();

            pr.Property(p => p.ProbabilityFlat)
                .HasPrecision(9, 6)
                .IsRequired();
        });

        builder.OwnsOne(x => x.Timeframe, tm =>
        {
            tm.Property(p => p.Value)
                .HasColumnName("Timeframe")
                .HasMaxLength(10)
                .IsRequired();
        });

        builder.OwnsMany(x => x.Indicators, ind =>
        {
            ind.ToTable("AnalyticsReportIndicators");

            ind.WithOwner().HasForeignKey("ReportId");

            ind.Property(i => i.Value)
                .IsRequired(false)
                .HasPrecision(18, 8);

            ind.Property(i => i.Importance)
                .HasPrecision(9, 6)
                .IsRequired();

            ind.Property(i => i.Name)
                .IsRequired()
                .HasMaxLength(100);

            ind.HasKey("ReportId", "Name");

            ind.HasIndex("ReportId");
        });

        builder.HasIndex(x => x.CoinPairId )
            .HasDatabaseName("AnalyticsReports_CoinPairId");

        builder.HasIndex("CoinPairId", "Timeframe", "CreatedAt")
            .HasDatabaseName("AnalyticsReports_CoinPair_Timeframe_CreatedAt");
    }
}
