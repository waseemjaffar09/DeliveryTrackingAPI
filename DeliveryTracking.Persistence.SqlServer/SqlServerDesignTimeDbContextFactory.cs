using DeliveryTracking.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace DeliveryTracking.Persistence.SqlServer
{
    /// <summary>
    /// Design-time factory used only by the EF Core tools (dotnet ef) when
    /// generating or applying SQL Server migrations from this assembly.
    /// It is never used at application runtime.
    /// </summary>
    public class SqlServerDesignTimeDbContextFactory : IDesignTimeDbContextFactory<DeliveryTrackingDbContext>
    {
        public DeliveryTrackingDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<DeliveryTrackingDbContext>();

            // A placeholder connection string is fine for generating migrations;
            // the actual connection string is supplied at runtime/update time.
            optionsBuilder.UseSqlServer(
                "Server=(localdb)\\MSSQLLocalDB;Database=DeliveryTrackingDb;Trusted_Connection=True;TrustServerCertificate=True;",
                sql => sql.MigrationsAssembly(typeof(SqlServerDesignTimeDbContextFactory).Assembly.GetName().Name));

            return new DeliveryTrackingDbContext(optionsBuilder.Options);
        }
    }
}
