using System.Collections.Immutable;
using BankSystem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClientStorage.EntityConfigurations;

public class AccountEntityTypeConfiguration : IEntityTypeConfiguration<Account>
{
    public void Configure(EntityTypeBuilder<Account> builder)
    {
        builder.ToTable("Accounts");
        builder.Property(a => a.ClientId).IsRequired();
        builder.Property(a => a.Id).IsRequired();
        builder.Property(a => a.CurrencyName).IsRequired();
        builder.Property(a => a.Amount).HasDefaultValue(0).IsRequired();

        builder.HasKey(a => a.Id);
        
        builder.HasOne(a => a.Client).WithMany(c =>
            c.Accounts).HasForeignKey(cl => cl.ClientId);

    }
}