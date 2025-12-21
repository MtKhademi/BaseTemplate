namespace ECommerceModule.Persistence.Context;

internal class ECommerceDbDataSeeder : IDataSeeder
{
    private readonly ECommerceDbContext _dbContext;
    private readonly ILogger<ECommerceDbDataSeeder> _logger;

    public ECommerceDbDataSeeder(ECommerceDbContext dbContext, ILogger<ECommerceDbDataSeeder> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task SeedAsync()
    {
        try
        {
            _logger.LogInformation("Seeding ECommerce database started");

            // Add seed data here if needed
            // Example: Seed default categories, products, etc.

            await _dbContext.SaveChangesAsync();
            _logger.LogInformation("ECommerce database seeding completed successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while seeding ECommerce database");
            throw;
        }
    }
}
