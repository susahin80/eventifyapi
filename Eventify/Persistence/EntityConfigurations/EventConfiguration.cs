using Eventify.Core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eventify.Persistence.Configurations
{
    public class EventConfiguration : IEntityTypeConfiguration<Event>
    {
        public void Configure(EntityTypeBuilder<Event> builder)
        {

            builder.Property(e => e.Title).IsRequired().HasMaxLength(30);

            builder.Property(e => e.Description).IsRequired().HasMaxLength(500);

            builder.Property(e => e.PlaceName).IsRequired().HasMaxLength(30);

            builder.Property(e => e.HostId).IsRequired();

            builder.Property(e => e.CategoryId).IsRequired();

            builder.Property(e => e.StartDate).IsRequired();

            builder.Property(e => e.EndDate).IsRequired();

            builder.HasOne(e => e.Category).WithMany(c => c.Events).HasForeignKey(e => e.CategoryId);

        }
    }
}



