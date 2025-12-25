using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities;

namespace Infrastructure.Data.Configurations;

public class AnalyticsReportConfiguration : IEntityTypeConfiguration<AnalyticsReport>
{
    public void Configure(EntityTypeBuilder<AnalyticsReport> builder)
    {
        builder.HasKey(t => t.Id);

        builder.Property(t => t.Coin)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(t => t.CurrentPrice)
            .IsRequired()
            .HasColumnType("decimal(18, 8)");

        builder.Property(t => t.ChangePercent)
            .IsRequired();

        builder.OwnsOne(t => t.Summary, summaryBuilder => //поля типа Recommendation встраиваются в таблицу AnalyticsReport, т.к. связь один-к-одному (каждому AnalyticsReport один Recommendation)
        {
            summaryBuilder.Property(t => t.Action)
                .IsRequired()
                .HasMaxLength(20);

            summaryBuilder.Property(t => t.Confidence)
                .IsRequired();

            summaryBuilder.Property(t => t.Risk)
                .IsRequired()
                .HasMaxLength(10);

            summaryBuilder.Property(t => t.Summary)
                .IsRequired()
                .HasMaxLength(1000);
        });

        builder.OwnsMany(t => t.Indicators, indicatorBuilder => //сохраняем indicator как json
        {
            indicatorBuilder.ToJson();
        });
    }
}
