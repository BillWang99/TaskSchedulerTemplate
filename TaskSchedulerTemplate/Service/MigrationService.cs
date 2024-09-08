using FluentMigrator.Runner;
using TaskSchedulerTemplate.Migrations;


namespace TaskSchedulerTemplate.Service
{
    public class MigrationService
    {
        private static IServiceProvider CreateService()
        {
            return new ServiceCollection()
                // Add common FluentMigrator services
                .AddFluentMigratorCore()
                .ConfigureRunner(rb => rb
                    // Add SQLite support to FluentMigrator
                    .AddSqlServer()
                    // Set the connection string
                    .WithGlobalConnectionString("Server = localhost; Database = TaskSchedulerTemplate; User Id = BWD; Password = Bluewhale09@;Trusted_Connection=True;TrustServerCertificate=true")
                    // Define the assembly containing the migrations
                    .ScanIn(typeof(UpdateTable_20240908).Assembly).For.Migrations())
                // Enable logging to console in the FluentMigrator way
                .AddLogging(lb => lb.AddFluentMigratorConsole())
                // Build the service provider
                .BuildServiceProvider(false);
        }

        private static void UpdateDatabase(IServiceProvider serviceProvider)
        {
            // Instantiate the runner
            var runner = serviceProvider.GetRequiredService<IMigrationRunner>();

            // Execute the migrations
            runner.MigrateUp();
        }

        

        public static void ApplyUpDateDB()
        {

            var serviceProvider = CreateService();
            using (var scope = serviceProvider.CreateScope())
            {
                UpdateDatabase(scope.ServiceProvider);
            }
        }

        
    }
}
