namespace IAMModule.Data.Configs;

internal class FeatureEntityConfig : IEntityTypeConfiguration<FeatureEntity>
{
    public void Configure(EntityTypeBuilder<FeatureEntity> builder)
    {
        builder.ToTable("Features", "IAM");

        builder.HasKey(af => af.Id);

        builder.Property(af => af.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(af => af.Description)
            .HasMaxLength(500);

        builder.HasMany(af => af.Permissions)
            .WithOne(ap => ap.Feature)
            .HasForeignKey(ap => ap.FeatureId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
