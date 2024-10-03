using Application;
using Application.Migrations;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;

var builder = WebApplication.CreateBuilder(args);

FirebaseApp.Create(
    new AppOptions() { Credential = GoogleCredential.FromFile("firebase_admin_sdk.json") }
);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddEndpoints();
builder.Services.AddRepositories();
builder.Services.AddPersistences(builder);
builder.Services.AddAuthentications(builder);
builder.Services.AddAuthorizations();

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

// This is required for the Firebase authentication scheme
app.UseAuthentication();
app.UseAuthorization();

// Health check endpoint
app.MapGet("/health", () => "Ok!");
app.MapEndpoints();

app.Run();
