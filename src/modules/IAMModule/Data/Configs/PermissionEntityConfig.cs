namespace IAMModule.Data.Configs;

internal class PermissionEntityConfig : IEntityTypeConfiguration<PermissionEntity>
{
    public void Configure(EntityTypeBuilder<PermissionEntity> builder)
    {
        builder.ToTable("Permissions", "IAM");

        builder.HasKey(ap => ap.Id);
        builder.Property(ap => ap.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(ap => ap.Description)
            .IsRequired()
            .HasMaxLength(500);
    
        builder.Property(ap => ap.Action)
            .IsRequired()
            .HasColumnType("nvarchar(50)");


    }
}
