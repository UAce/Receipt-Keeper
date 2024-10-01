using System.Reflection;
using Core;
using Core.Interfaces;
using DbUp;
using Infrastructure.Repositories;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.IdentityModel.Tokens;

// Temporary solution to get the Configs
IConfiguration configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true)
    .Build();

// Console.WriteLine(configuration);
// Console.WriteLine(configuration.GetConnectionString("DefaultConnection"));

var upgradeEngine = DeployChanges.To.PostgresqlDatabase(configuration.GetConnectionString("DefaultConnection"))
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
        Console.ResetColor();
        System.Environment.Exit(1);
    }
    
    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine(value: "Migration Successfully Ran!");
    Console.ResetColor();
}

var builder = WebApplication.CreateBuilder(args);

// Inject configuration as singleton
builder.Services.AddSingleton(configuration);
    
// Configure JSON logging to the console
// builder.Logging.AddJsonConsole();

// Add services to the container.
var firebaseProjectId = configuration["Firebase:projectId"];
builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = $"https://securetoken.google.com/{firebaseProjectId}";
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = $"https://securetoken.google.com/{firebaseProjectId}",
            ValidateAudience = true,
            ValidAudience = $"{firebaseProjectId}",
            ValidateLifetime = true,
            RequireSignedTokens = true,
        };
    });

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure dependency injection
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddMediatR(typeof(MediatorEntrypoint).Assembly);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Forward headers from proxy servers
app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});

// Commenting this out because it was causing the app to be unreachable in Production
// app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();