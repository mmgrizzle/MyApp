using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyApp.Persistance.Configurations
{
    public class ProfessorConfiguration : IEntityTypeConfiguration<Professor>
    {
        public void Configure(EntityTypeBuilder<Professor> builder)
        {
            builder.HasKey(e => e.ProfessorId);

            builder.Property(e => e.ProfessorId).HasColumnName("ProfessorID");

            builder.Property(e => e.ProfessorName)
                .IsRequired()
                .HasMaxLength(35);
        }
    }
}
