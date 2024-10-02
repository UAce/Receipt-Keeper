using Features.Endpoints;
using Features.Migrations;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddEndpoints();

var app = builder.Build();

// Run database migrations
app.MigrateDatabase<Program>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    Console.WriteLine("http://localhost:5103/swagger");
}

// Don't redirect to HTTPS, we'll be using a reverse proxy in production
// app.UseHttpsRedirection();

// Health check endpoint
app.MapGet("/health", () => "Ok!");
app.MapEndpoints();

app.Run();
