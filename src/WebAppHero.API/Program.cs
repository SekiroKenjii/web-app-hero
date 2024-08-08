using WebAppHero.API.DependencyInjection.Extensions;
using WebAppHero.API.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.ConfigureApplicationLogger();

// Add services to the container.
builder.Services.AddHttpContextAccessor();
builder.Services.AddJwtAuthentication(builder.Configuration, builder.Environment);
builder.Services.AddApiConfigurations();
builder.Services.AddSwagger();
builder.Services.AddApplicationLayer();
builder.Services.AddInfrastructureLayer(builder.Configuration);
builder.Services.AddPersistenceLayer(builder.Environment);

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseMiddleware<ExceptionHandlerMiddleware>();
app.ConfigureSwagger(app.Environment);
app.UseAuthentication();
app.UseAuthorization();
app.MapApiEndpoints();

await app.RunApplicationAsync();
