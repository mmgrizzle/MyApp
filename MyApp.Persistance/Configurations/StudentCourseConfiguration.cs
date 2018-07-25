using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyApp.Persistance.Configurations
{
    public class StudentCourseConfiguration : IEntityTypeConfiguration<StudentCourse>
    {
        public void Configure(EntityTypeBuilder<StudentCourse> builder)
        {
            builder.HasKey(x => new { x.StudentId, x.CourseId });

            builder.HasOne(x => x.Student)
                .WithMany(x => x.Courses)
                .HasForeignKey(x => x.CourseId);

            builder.HasOne(x => x.Course)
                .WithMany(y => y.Students)
                .HasForeignKey(y => y.StudentId);
        }
    }
}
