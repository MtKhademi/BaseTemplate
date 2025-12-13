namespace IAMModule.Data.Configs;

internal class ApplicationUserLoginConfig :
    IEntityTypeConfiguration<IdentityUserLogin<string>>
{
    public void Configure(EntityTypeBuilder<IdentityUserLogin<string>> builder)
    {
        builder.ToTable("UserLogins", "IAM");
    }
}
