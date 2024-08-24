using Cachier.net.Extensions;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddApplicationServices(builder.Configuration);
var app = builder.Build();
// Configure the HTTP request pipeline.
app.UseApplicationConfiguration();
app.Run();