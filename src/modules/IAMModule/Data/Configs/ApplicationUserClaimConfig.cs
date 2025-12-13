namespace IAMModule.Data.Configs;

internal class ApplicationUserClaimConfig :
    IEntityTypeConfiguration<IdentityUserClaim<string>>
{
    public void Configure(EntityTypeBuilder<IdentityUserClaim<string>> builder)
    {
        builder.ToTable("UserClaims", "IAM");
    }
}
