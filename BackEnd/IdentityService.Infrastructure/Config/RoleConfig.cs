namespace IdentityService.Infrastructure.Config;

public class RoleConfig : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.HasKey(e => e.Id).IsClustered(false);
        builder.HasIndex(e => e.Name).IsUnique(true);
    }
}
