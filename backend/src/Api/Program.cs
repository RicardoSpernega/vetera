using Api.Middlewares;
using Application.DependencyInjection;
using Infrastructure.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);
const string FrontendDevCorsPolicy = "FrontendDev";

var allowedCorsOrigins = builder.Configuration
    .GetSection("Cors:AllowedOrigins")
    .Get<string[]>()
    ?? ["http://localhost:4200", "http://127.0.0.1:4200"];

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddPolicy(FrontendDevCorsPolicy, corsBuilder =>
    {
        corsBuilder
            .WithOrigins(allowedCorsOrigins)
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services
    .AddApplication()
    .AddInfrastructure(builder.Configuration);

var app = builder.Build();

app.UseMiddleware<GlobalExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors(FrontendDevCorsPolicy);
app.UseAuthorization();
app.MapControllers();

app.Run();

public partial class Program;
