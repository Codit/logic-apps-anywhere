using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Sello.OrderService
{
    public class Program
    {
        public static int Main(string[] args)
        {
            CreateHostBuilder(args)
                .Build()
                .Run();

            return 0;
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            IConfiguration configuration = CreateConfiguration(args);
            IHostBuilder webHostBuilder = CreateHostBuilder(args, configuration);

            return webHostBuilder;
        }

        private static IConfiguration CreateConfiguration(string[] args)
        {
            IConfigurationRoot configuration =
                new ConfigurationBuilder()
                    .AddCommandLine(args)
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .AddEnvironmentVariables()
                    .Build();

            return configuration;
        }

        private static IHostBuilder CreateHostBuilder(string[] args, IConfiguration configuration)
        {
            string httpEndpointUrl = "http://+:" + configuration["ARCUS_HTTP_PORT"];
            IHostBuilder webHostBuilder =
                Host.CreateDefaultBuilder(args)
                    .ConfigureAppConfiguration(configBuilder => configBuilder.AddConfiguration(configuration))
                    .ConfigureSecretStore((config, stores) =>
                    {
//#if DEBUG
                        stores.AddConfiguration(config);
//#endif

                        //#error Please provide a valid secret provider, for example Azure Key Vault: https://security.arcus-azure.net/features/secrets/consume-from-key-vault
                        stores.AddAzureKeyVaultWithManagedServiceIdentity("https://your-keyvault-vault.azure.net/");
                    })
                    .ConfigureWebHostDefaults(webBuilder =>
                    {
                        webBuilder.ConfigureKestrel(kestrelServerOptions => kestrelServerOptions.AddServerHeader = false)
                                  .UseUrls(httpEndpointUrl)
                                  .ConfigureLogging(logging => logging.AddConsole())
                                  .UseStartup<Startup>();
                    });

            return webHostBuilder;
        }
    }
}
