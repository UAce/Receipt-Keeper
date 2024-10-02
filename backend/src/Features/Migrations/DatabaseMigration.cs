using System.Reflection;
using DbUp;

namespace Features.Migrations;

public static class DatabaseMigration
{
        public static IHost MigrateDatabase<TContext>(this IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var configuration = services.GetRequiredService<IConfiguration>();

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("===== Migrating PostgreSQL database =====");
                Console.ResetColor();

                string connection = configuration.GetConnectionString("DefaultConnection") 
                                     ?? throw new InvalidOperationException("Connection string 'DefaultConnection' is null.");

                // This prints out the connection string to the console
                // EnsureDatabase.For.PostgresqlDatabase(connection);

                var upgradeEngine = DeployChanges.To
                    .PostgresqlDatabase(connection)
                    .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
                    .LogToConsole()
                    .Build();


                upgradeEngine.GetScriptsToExecute();

                if (upgradeEngine.IsUpgradeRequired())
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("Running migration...");
                    var result = upgradeEngine.PerformUpgrade();

                    if (!result.Successful)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(result.Error);
                        Console.WriteLine("===== An error occurred while running migration =====");
                        Console.ResetColor();
                        return host;
                    }
                    
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("===== Migration Successfully Ran! =====");
                    Console.ResetColor();
                } else {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("===== No migration required. Skipping... =====");
                    Console.ResetColor();
                }
            }

        return host;
     }
}