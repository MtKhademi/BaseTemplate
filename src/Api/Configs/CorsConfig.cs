namespace Api.Configs
{
    public static class CorsConfig
    {
        private const string allowEveryOne = "AllowAll";

        public static IServiceCollection AddCorsConfig(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", builder =>
                {
                    builder
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                });
            });
            return services;
        }

        public static IApplicationBuilder UseCorsConfig(this IApplicationBuilder app)
        {
            app.UseCors(allowEveryOne);
            return app;
        }

    }
}
