namespace Remake_Kafka_Example_01.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddServicesCollection(this IServiceCollection services, IConfiguration configuration, AppSetting appSetting)
        {
            services.AddDbContextServices(configuration)
                    .AddStackExchangeRedisCacheService(configuration)
                    .AddSingletonServices()
                    .AddScopedService()
                    .AddMemoryCommandHandlers()
                    .AddDatabaseCommandHandlers()
                    .AddKafkaServices(appSetting)
                    .AddMiscellaneousServices();

            return services;
        }

        public static IServiceCollection AddDbContextServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(opts =>
            {
                opts.UseOracle(configuration.GetConnectionString("OrclDB"));
            });

            return services;
        }

        public static IServiceCollection AddStackExchangeRedisCacheService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddStackExchangeRedisCache(opt =>
            {
                opt.Configuration = configuration.GetConnectionString("Redis");
            });

            return services;
        }

        public static IServiceCollection AddSingletonServices(this IServiceCollection services)
        {
            services.AddSingleton<IProductPersistanceService, ProductPersistanceService>();
            services.AddSingleton<IProductService,            ProductService>();
            services.AddSingleton<IProductProducer,           ProductProducer>();

            return services;
        }

        public static IServiceCollection AddScopedService(this IServiceCollection services)
        {
            services.AddScoped<ICacheService, CacheService>();

            return services;
        }

        public static IServiceCollection AddMemoryCommandHandlers(this IServiceCollection services)
        {
            services.AddTransient<IMemoryCommandHandler<InsertProductToMemoryCommand>,  InsertProductToMemoryCommandHandler>();
            services.AddTransient<IMemoryCommandHandler<UpdatePriceToMemoryCommand>,    UpdatePriceToMemoryCommandHandler>();
            services.AddTransient<IMemoryCommandHandler<UpdateQuantityToMemoryCommand>, UpdateQuantityToMemoryCommandHandler>();

            return services;
        }

        public static IServiceCollection AddDatabaseCommandHandlers(this IServiceCollection services)
        {
            services.AddTransient<IDatabaseCommandHandler<InsertProductToDatabaseCommand>,  InsertProductToDatabaseCommandHandler>();
            services.AddTransient<IDatabaseCommandHandler<UpdatePriceToDatabaseCommand>,    UpdatePriceToDatabaseCommandHandler>();
            services.AddTransient<IDatabaseCommandHandler<UpdateQuantityToDatabaseCommand>, UpdateQuantityToDatabaseCommandHandler>();

            return services;
        }

        public static IServiceCollection AddKafkaServices(this IServiceCollection services, AppSetting appSetting)
        {
            services.AddKafkaProducers(producerBuilder =>
            {
                producerBuilder.AddProducer(appSetting.GetProducerSetting("1"));
            });

            services.AddKafkaConsumers(builder =>
            {
                builder.AddConsumer<ProductConsumingTask>(appSetting.GetConsumerSetting("0"));
                builder.AddConsumer<ProductPersistanceConsumingTask>(appSetting.GetConsumerSetting("1"));
            });

            return services;
        }

        public static IServiceCollection AddMiscellaneousServices(this IServiceCollection services)
        {
            services.AddMemoryCache();
            return services;
        }
    }
}
