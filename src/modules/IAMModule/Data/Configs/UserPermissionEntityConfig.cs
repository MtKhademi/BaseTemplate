namespace IAMModule.Data.Configs;

internal class UserPermissionEntityConfig : IEntityTypeConfiguration<UserAppPermissionEntity>
{
    public void Configure(EntityTypeBuilder<UserAppPermissionEntity> builder)
    {
        builder.ToTable("UserPermissions", "IAM");
    
        builder.HasKey(x => new { x.UserId, x.AppPermissionId });
    }
}
