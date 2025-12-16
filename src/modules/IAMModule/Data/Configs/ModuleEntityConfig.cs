namespace IAMModule.Data.Configs;

internal class ModuleEntityConfig : IEntityTypeConfiguration<ModuleEntity>
{
    public void Configure(EntityTypeBuilder<ModuleEntity> builder)
    {
        builder.ToTable("Modules", "IAM");

        builder.HasKey(af => af.Id);

        builder.Property(af => af.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(af => af.Description)
            .HasMaxLength(500);

        builder.HasMany(af => af.Features)
            .WithOne(ap => ap.Module)
            .HasForeignKey(ap => ap.ModuleId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(af => af.Name)
            .IsUnique();
    }
}
