namespace NotificationModule.Data.Context.Configs;

internal class ApplicationUserConfig : IEntityTypeConfiguration<NotificationEntity>
{
    public void Configure(EntityTypeBuilder<NotificationEntity> builder)
    {
        builder.ToTable("Notifications","Notification");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.UserId);
        builder.Property(x => x.Type).IsRequired();
        builder.Property(x => x.Status).IsRequired();

        builder.Property(x => x.Destination)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(x => x.Payload)
            .HasMaxLength(500);

        builder.Property(x => x.Provider).HasMaxLength(100);

        builder.Property(x => x.ProviderRequest);
        builder.Property(x => x.ProviderResponse);
        builder.Property(x => x.ErrorMessage);

        builder.Property(x => x.RequestedAtUtc).IsRequired();
    }
}
