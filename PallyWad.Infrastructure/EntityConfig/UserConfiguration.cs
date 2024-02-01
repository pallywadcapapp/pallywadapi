using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.DependencyInjection;
using PallyWad.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PallyWad.Infrastructure.EntityConfig
{
    public class UserConfiguration : IEntityTypeConfiguration<AppIdentityUser>
    {
        public void Configure(EntityTypeBuilder<AppIdentityUser> builder)
        {
            /*builder
           .HasOne(a => a.UserProfile)
           .WithOne(a => a.AppIdentityUser)
           .HasForeignKey<UserProfile>(c => c.memberid);*/

            builder.ToTable("AspNetUsers");
            builder
            .HasMany(a => a.account)
            .WithOne(a => a.member);

            
    }
    }
}
