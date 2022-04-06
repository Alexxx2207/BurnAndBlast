namespace Ignite.Data
{
    using System.IO;
    using Microsoft.Data.SqlClient;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Design;
    using Microsoft.Extensions.Configuration;
    using Azure.Security.KeyVault.Secrets;
    using Azure.Identity;

    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {

        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            var builder = new DbContextOptionsBuilder<ApplicationDbContext>();
            var connectionString = new SqlConnectionStringBuilder(configuration.GetConnectionString("DefaultConnection"));

            var keyVaultEndpoint = new Uri(Environment.GetEnvironmentVariable("VaultUri"));

            SecretClient _secretClient = new SecretClient(keyVaultEndpoint, new DefaultAzureCredential());

            KeyVaultSecret keyValueSecret = _secretClient.GetSecretAsync("ConnectionStringPasswordAzure").Result;

            connectionString.Password = keyValueSecret.Value;

            builder.UseSqlServer(connectionString.ConnectionString);

            return new ApplicationDbContext(builder.Options);
        }
    }
}
