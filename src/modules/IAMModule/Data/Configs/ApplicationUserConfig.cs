namespace IAMModule.Data.Configs;

internal class ApplicationUserConfig : IEntityTypeConfiguration<ApplicationUser>
{
    public ApplicationUserConfig()
    {
    }

    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder.ToTable("Users", "IAM");
    }
}
