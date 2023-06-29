namespace IdentityService.Infrastructure.Config;

public class UserInformationConfig : IEntityTypeConfiguration<UserInformation>
{
    public void Configure(EntityTypeBuilder<UserInformation> builder)
    {
        builder.HasKey(e => e.Id).IsClustered(false);
        builder.HasIndex(e => e.UserId).IsUnique(true);
    }
}
