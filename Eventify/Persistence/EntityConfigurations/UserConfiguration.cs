using Eventify.Core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eventify.Persistence.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {

            builder.Property(u => u.Username).IsRequired().HasMaxLength(20);

            builder.Property(u => u.Email).IsRequired().HasMaxLength(100);

            builder.Property(u => u.Password).IsRequired().HasMaxLength(200);

            builder.Property(u => u.Gender).IsRequired().HasMaxLength(1);

            builder.Property(u => u.Role).HasMaxLength(10).HasDefaultValue("user");

            builder.Property(u => u.BirthDate).IsRequired();

            builder.HasMany(u => u.Events).WithOne(e => e.Host).HasForeignKey(e => e.HostId);

        }
    }
}
