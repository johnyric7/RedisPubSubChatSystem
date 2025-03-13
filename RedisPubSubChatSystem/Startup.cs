using System;
using StackExchange.Redis;
using Amazon.DynamoDBv2;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using RedisPubSubChatSystem.Services;

namespace RedisPubSubChatSystem
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            bool runLocalDynamoDb = environment == "Development"; // Set to true for development, false for production

            if (runLocalDynamoDb)
            {
                services.AddSingleton<IAmazonDynamoDB>(sp =>
                {
                    var clientConfig = new AmazonDynamoDBConfig
                    {
                        ServiceURL = Configuration.GetValue<string>("DynamoDb:LocalServiceUrl")
                    };
                    return new AmazonDynamoDBClient(clientConfig);
                });
            }
            else
            {
                services.AddAWSService<IAmazonDynamoDB>();
            }

            services.AddSignalR();  // Register SignalR services
            services.AddSingleton<IConnectionMultiplexer>(sp =>
            {
                // This ensures that the connection is long-lived.
                return ConnectionMultiplexer.Connect("localhost:6379");
            });

            services.AddSingleton<IRedisService, RedisService>();
            services.AddSingleton<IDynamoDbService, DynamoDbService>();

            services.AddControllers();

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseStaticFiles();

            // Map the SignalR hub to the "/chat" URL
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<ChatHub>("/chat");
            });
        }
    }
}
