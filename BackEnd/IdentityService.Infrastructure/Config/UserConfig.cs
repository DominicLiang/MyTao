namespace IdentityService.Infrastructure.Config;

public class UserConfig : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(e => e.Id).IsClustered(false);
        builder.HasIndex(e => e.UserName).IsUnique(true);
        builder.HasIndex(e => e.Email).IsUnique(true);
        builder.HasIndex(e => e.PhoneNumber).IsUnique(true);
    }
}
