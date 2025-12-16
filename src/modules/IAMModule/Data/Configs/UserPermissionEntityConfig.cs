namespace IAMModule.Data.Configs;

internal class UserPermissionEntityConfig : IEntityTypeConfiguration<UserPermissionEntity>
{
    public void Configure(EntityTypeBuilder<UserPermissionEntity> builder)
    {
        builder.ToTable("UserPermissions", "IAM");
    
        builder.HasKey(x => new { x.UserId, x.PermissionId });
    }
}
