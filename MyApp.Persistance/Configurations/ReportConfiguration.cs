using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyApp.Persistance.Configurations
{
    public class ReportConfiguration : IEntityTypeConfiguration<Report> 
    {
        public void Configure(EntityTypeBuilder<Report> builder)
        {
            builder.HasKey(r => r.ReportId);

            builder.Property(r => r.ReportId).HasColumnName("ReportID");

            builder.HasOne(s => s.Student)
                .WithMany(r => r.Reports)
                .HasForeignKey(f => f.StudentId)
                .HasConstraintName("FK_Reports_Students");
        }
    }
}
