using BankSystem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClientStorage.EntityConfigurations;

public class EmployeeEntityTypeConfiguration : IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        builder.ToTable("Employees");
        builder.Property(e => e.Id);
        builder.Property(e => e.Name).HasMaxLength(100).IsRequired();
        builder.Property(e => e.SecondName).IsRequired();
        builder.Property(e => e.ThirdName);
        builder.Property(e => e.Age).IsRequired();
        builder.Property(e => e.PasNumber).IsRequired();
        builder.Property(e => e.PhoneNumber).IsRequired();
        
        builder.Property(e => e.Contract).IsRequired();
        builder.Property(e => e.Salary).IsRequired();
        builder.Property(e => e.IsOwner).HasDefaultValue(false).IsRequired();

        builder.HasKey(e => e.Id);
    }
}