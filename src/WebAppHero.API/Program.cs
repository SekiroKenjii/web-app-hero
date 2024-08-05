using WebAppHero.API.DependencyInjection.Extensions;
using WebAppHero.API.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.ConfigureApplicationLogger();

// Add services to the container.
builder.Services.AddApiConfigurations();
builder.Services.AddSwagger();
builder.Services.AddApplicationLayer();
builder.Services.AddPersistenceLayer();

var app = builder.Build();
var env = app.Environment;

// Configure the HTTP request pipeline.
app.UseMiddleware<ExceptionHandlerMiddleware>();
app.MapApiEndpoints();
app.ConfigureSwagger(env);

await app.RunApplicationAsync();
