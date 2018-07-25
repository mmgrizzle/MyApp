using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyApp.Persistance.Configurations
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Course>
    {
        public void Configure(EntityTypeBuilder<Course> builder)
        {
            builder.HasKey(e => e.CourseId);

            builder.Property(e => e.CourseId).HasColumnName("CourseID");

            builder.Property(e => e.CourseName)
                .IsRequired()
                .HasMaxLength(30);

            builder.HasOne(d => d.Professor)
                .WithMany(p => p.Courses)
                .HasForeignKey(d => d.ProfessorId)
                .HasConstraintName("FK_Courses_Professors");
        }
    }
}
