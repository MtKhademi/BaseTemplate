namespace IAMModule.Data.Configs;

internal class ApplicationUserRoleConfig :
    IEntityTypeConfiguration<IdentityUserRole<string>>
{
    public void Configure(EntityTypeBuilder<IdentityUserRole<string>> builder)
    {
        builder.ToTable("UserRoles", "IAM");
    }
}
