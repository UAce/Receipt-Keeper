using System.Reflection;
using DbUp;
using DbUp.Helpers;

namespace Application.Migrations;

public static class DatabaseMigration
{
        public static IHost MigrateDatabase<TContext>(this IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var configuration = services.GetRequiredService<IConfiguration>();

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("===== Checking Database Migrations =====");
                Console.ResetColor();

                string dbConnectionString = configuration.GetConnectionString("DefaultConnection") 
                                     ?? throw new InvalidOperationException("Connection string 'DefaultConnection' is null.");

                // This prints out the connection string to the console
                // EnsureDatabase.For.PostgresqlDatabase(dbConnectionString);

                var upgradeEngine = DeployChanges.To
                    .PostgresqlDatabase(dbConnectionString)
                    .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
                    .JournalTo(new NullJournal()) // This allows always running the migrations
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