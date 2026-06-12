using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HealthController : ControllerBase
{
    private readonly IConfiguration _configuration;

    public HealthController(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    [HttpGet]
    public IActionResult Get() => Ok(new
    {
        message = "Backend is up and running.",
        databaseConfigured = !string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable("SQLSERVER_CONNECTION_STRING")
            ?? _configuration["SqlServerConnectionString"]
            ?? _configuration.GetConnectionString("DefaultConnection"))
    });

    [HttpGet("db")]
    public async Task<IActionResult> DbCheck()
    {
        var connectionString = Environment.GetEnvironmentVariable("SQLSERVER_CONNECTION_STRING")
            ?? _configuration["SqlServerConnectionString"]
            ?? _configuration.GetConnectionString("DefaultConnection");

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            return BadRequest(new { status = "missing-connection-string", message = "Set SqlServerConnectionString or SQLSERVER_CONNECTION_STRING." });
        }

        try
        {
            await using var connection = new SqlConnection(connectionString);
            await connection.OpenAsync();

            return Ok(new
            {
                status = "ok",
                server = connection.DataSource,
                database = connection.Database,
                message = "SQL Server connection is available."
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { status = "error", message = ex.Message });
        }
    }
}
