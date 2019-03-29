using System.IO;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureKeyVault;

namespace ComicBookRepository {

    public class Program {

        #region Public Methods

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((context, config) => {
                    var env = context.HostingEnvironment;
                    config.SetBasePath(Directory.GetCurrentDirectory());
                    config.AddJsonFile("appsettings.json", false, true);
                    config.AddJsonFile($"appsettings.{env.EnvironmentName}.json", true, true);
                    var builtConfig = config.Build();
                    var azureServiceTokenProvider = new AzureServiceTokenProvider();
                    var keyVaultClient = new KeyVaultClient((authority, resource, scope) => azureServiceTokenProvider.KeyVaultTokenCallback(authority, resource, scope));
                    config.AddAzureKeyVault(

                        builtConfig["KeyVault:BaseUrl"],
                        keyVaultClient,
                        new DefaultKeyVaultSecretManager());
                }
                    )
                .UseStartup<Startup>();

        public static void Main(string[] args) {
            CreateWebHostBuilder(args).Build().Run();
        }

        #endregion Public Methods
    }
}