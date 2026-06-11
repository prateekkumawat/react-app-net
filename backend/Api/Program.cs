using Microsoft.Data.SqlClient;

var builder = WebApplication.CreateBuilder(args);

var connectionString =
    Environment.GetEnvironmentVariable("SQLSERVER_CONNECTION_STRING")
    ?? builder.Configuration["SqlServerConnectionString"];

builder.Services.AddControllers();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

app.UseRouting();
app.UseCors("AllowFrontend");
app.MapControllers();

app.MapGet("/api/health", () =>
    Results.Ok(new
    {
        message = "Backend is up and running.",
        databaseConnectionStringConfigured = !string.IsNullOrWhiteSpace(connectionString)
    }));

app.MapGet("/api/db-check", async () =>
{
    if (string.IsNullOrWhiteSpace(connectionString))
    {
        return Results.BadRequest(new { status = "missing-connection-string", message = "Set SqlServerConnectionString or SQLSERVER_CONNECTION_STRING." });
    }

    try
    {
        await using var connection = new SqlConnection(connectionString);
        await connection.OpenAsync();

        return Results.Ok(new
        {
            status = "ok",
            server = connection.DataSource,
            database = connection.Database,
            message = "SQL Server connection is available."
        });
    }
    catch (Exception ex)
    {
        return Results.Problem(detail: ex.Message, title: "Database connection failed");
    }
});

app.Run();
