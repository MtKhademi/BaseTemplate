namespace IAMModule.Data.Configs;

internal class ApplicationUserTokenConfig :
    IEntityTypeConfiguration<IdentityUserToken<string>>
{
    public void Configure(EntityTypeBuilder<IdentityUserToken<string>> builder)
    {
        builder.ToTable("UserTokens", "IAM");
    }
}