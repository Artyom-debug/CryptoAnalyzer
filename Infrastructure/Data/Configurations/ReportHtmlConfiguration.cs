using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class ReportHtmlConfiguration : IEntityTypeConfiguration<ReportHtml>
{
    public void Configure(EntityTypeBuilder<ReportHtml> builder)
    {
        builder.HasKey(t => t.Id);

        builder.HasOne(t => t.Report)
            .WithOne()
            .HasForeignKey<ReportHtml>(t => t.Id);

        builder.Property(t => t.HtmlContent)
            .IsRequired();

        builder.Property("IsCurrent")
            .IsRequired()
            .HasDefaultValue(true);

        builder.HasIndex("IsCurrent");
    }
}

