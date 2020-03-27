using Eventify.Core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eventify.Persistence.EntityConfigurations
{
    public class AttendanceConfiguration : IEntityTypeConfiguration<Attendance>
    {
        public void Configure(EntityTypeBuilder<Attendance> builder)
        {
            builder.Property(a => a.CreatedAt).IsRequired().HasDefaultValue(DateTime.Now);

            builder.HasKey(a => a.Id);

            // builder.HasKey(a => new { a.AttendeeId, a.EventId });

            builder.HasOne(a => a.Attendee).WithMany(u => u.Attendances).HasForeignKey(u => u.AttendeeId).OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(a => a.Event).WithMany(a => a.Attendances).HasForeignKey(u => u.EventId).OnDelete(DeleteBehavior.Restrict);

        }
    }
}
