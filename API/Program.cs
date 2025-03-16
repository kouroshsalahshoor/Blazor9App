var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Allow requests from the Blazor WASM host from JavaScript (JS interop is used by WASM) 
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowWASMOrigin",
                      policy =>
                      {
                          policy.WithOrigins("https://localhost:7001",
                            "http://localhost:5001");
                      });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowWASMOrigin");

app.UseHttpsRedirection();

// define an API for testing
app.MapGet("remoteapi/bands", () =>
{
    return Results.Ok(new[] { new { Id = 1, Name = "Arctic Monkeys (from remote API)" },
        new { Id = 2, Name = "Nine Inch Nails (from remote API)" },
        new { Id = 3, Name = "Bruce Springsteen (from remote API)" },
        new { Id = 4, Name = "Fleetwood Mac (from remote API)" } });
});

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");

app.Run();

internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
