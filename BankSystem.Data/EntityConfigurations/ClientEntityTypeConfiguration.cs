using BankSystem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClientStorage.EntityConfigurations;

public class ClientEntityTypeConfiguration : IEntityTypeConfiguration<Client>
{
    
    public void Configure(EntityTypeBuilder<Client> builder)
    {
        builder.ToTable("Clients");
        builder.Property(c => c.Id);
        builder.Property(c => c.Name).HasMaxLength(100).IsRequired();
        builder.Property(c => c.SecondName).IsRequired();
        builder.Property(c => c.ThirdName);
        builder.Property(c => c.Age).HasDefaultValue(0).IsRequired();
        builder.Property(c => c.PasNumber).IsRequired();
        builder.Property(c => c.PhoneNumber).IsRequired();
        
        builder.HasKey(с => с.Id);
        
        builder.Property(c => c.AccountNumber);
        builder.Property(c => c.Balance).HasDefaultValue(0);
    }
}