using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using ElysiaAPI.Application.Services;
using ElysiaAPI.Domain.Repositories;
using ElysiaAPI.Infrastructure.Mongo;
using ElysiaAPI.Infrastructure.Repositories;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<MongoSettings>(builder.Configuration.GetSection("Mongo"));
builder.Services.AddSingleton<MongoDbContext>();

builder.Services.AddScoped<IMotoRepository, MotoRepositoryMongo>();
builder.Services.AddScoped<IVagaRepository, VagaRepositoryMongo>();

builder.Services.AddScoped<MotoService>();
builder.Services.AddScoped<VagaService>();

builder.Services.AddControllers();
builder.Services.AddRouting(o => o.LowercaseUrls = true);

builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
})
.AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opt =>
{
    opt.SwaggerDoc("v1", new OpenApiInfo { Title = "Elysia API", Version = "v1", Description = "API v1" });
    opt.SwaggerDoc("v2", new OpenApiInfo { Title = "Elysia API", Version = "v2", Description = "API v2" });
});

builder.Services.AddHealthChecks()
    .AddMongoDb(builder.Configuration["Mongo:ConnectionString"]!, name: "mongodb", timeout: TimeSpan.FromSeconds(3));

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var ctx = scope.ServiceProvider.GetRequiredService<MongoDbContext>();
    await ElysiaAPI.Infrastructure.Mongo.MongoInitializer.EnsureAsync(ctx);
}

app.UseSwagger();

var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
app.UseSwaggerUI(opt =>
{
    foreach (var desc in provider.ApiVersionDescriptions)
        opt.SwaggerEndpoint($"/swagger/{desc.GroupName}/swagger.json", $"Elysia API {desc.GroupName}");
    opt.RoutePrefix = "swagger";
});

app.MapControllers();
app.MapHealthChecks("/health");

app.Run();
