using Api.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddNecessaryServices(builder.Configuration);
builder.Services.AddJwt(builder.Configuration);

var app = builder.Build();

app.UseNecessaryMiddleware();

app.Run();
