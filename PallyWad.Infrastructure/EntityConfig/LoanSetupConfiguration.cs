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
    public class LoanSetupConfiguration: IEntityTypeConfiguration<LoanSetup>
    {
        public void Configure(EntityTypeBuilder<LoanSetup> builder)
        {

            builder
            .HasMany(a => a.LoanDocuments)
            .WithOne(a => a.loanSetup);

            builder
                .HasMany(a => a.LoanCollaterals)
                .WithOne(a => a.loanSetup);
        }
    }
}
