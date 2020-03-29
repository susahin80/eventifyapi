using Eventify.Core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eventify.Persistence.EntityConfigurations
{
    public class FollowingConfiguration : IEntityTypeConfiguration<Following>
    {
        public void Configure(EntityTypeBuilder<Following> builder)
        {

            builder.HasKey(a => a.Id);

            builder.HasOne(a => a.Follower).WithMany(u => u.Followers).HasForeignKey(u => u.FollowedId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(a => a.Followed).WithMany(u => u.Followings).HasForeignKey(u => u.FollowerId).OnDelete(DeleteBehavior.Restrict);

        }
    }
 
}
