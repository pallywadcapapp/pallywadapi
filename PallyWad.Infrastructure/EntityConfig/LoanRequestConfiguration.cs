using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PallyWad.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PallyWad.Infrastructure.EntityConfig
{
    public class LoanRequestConfiguration : IEntityTypeConfiguration<LoanRequest>
    {
        public void Configure(EntityTypeBuilder<LoanRequest> builder)
        {
            builder
           .HasMany(a => a.loanUserCollaterals)
           .WithOne(a => a.loanRequest);

            builder
                .HasMany(a => a.loanUserDocuments)
                .WithOne(a => a.loanRequest);
        }
    }
}
