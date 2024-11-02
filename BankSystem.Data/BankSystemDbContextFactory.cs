using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace ClientStorage
{
    public class BankSystemDbContextFactory : IDesignTimeDbContextFactory<BankSystemDbContext>
    {
        public BankSystemDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<BankSystemDbContext>();
            optionsBuilder.UseNpgsql("Host=localhost;Port=5434;Username=postgres;Password=mysecretpassword;Database=local");

            return new BankSystemDbContext(optionsBuilder.Options);
        }
    }
}