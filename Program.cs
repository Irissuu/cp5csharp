
using ElysiaAPI.Domain.Repositories;
using ElysiaAPI.Infrastructure.Mongo;
using ElysiaAPI.Infrastructure.Repositories;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<MongoSettings>(builder.Configuration.GetSection("Mongo"));
builder.Services.AddSingleton<MongoDbContext>();

builder.Services.AddScoped<IMotoRepository, MotoRepositoryMongo>();
builder.Services.AddScoped<IVagaRepository, VagaRepositoryMongo>();



builder.Services.AddControllers();
builder.Services.AddRouting(o => o.LowercaseUrls = true);


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Elysia API", Version = "v1", Description = "API v1" });
    c.SwaggerDoc("v2", new OpenApiInfo { Title = "Elysia API", Version = "v2", Description = "API v2" });
});

builder.Services.AddHealthChecks()
    .AddMongoDb(builder.Configuration["Mongo:ConnectionString"]!, name: "mongodb");

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Elysia API v1");
    c.SwaggerEndpoint("/swagger/v2/swagger.json", "Elysia API v2");
    c.RoutePrefix = "swagger";
});

app.MapControllers();
app.MapHealthChecks("/health");

app.Run();