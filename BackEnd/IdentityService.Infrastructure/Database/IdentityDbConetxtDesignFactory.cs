namespace IdentityService.Infrastructure.Database;

public class IdentityDbConetxtDesignFactory : IDesignTimeDbContextFactory<IdentityDbContext>
{
    public IdentityDbContext CreateDbContext(string[] args)
    {
        DbContextOptionsBuilder<IdentityDbContext> builder = new DbContextOptionsBuilder<IdentityDbContext>();

        builder.UseSqlServer("Server=localhost;Database=mytao;User ID=dominic;Password=eurekaseven1987;TrustServerCertificate=true;");

        IdentityDbContext context = new IdentityDbContext(builder.Options);
        return context;
    }
}
