using Heritage.Application;
using Heritage.Persistance;
using Heritage.WebApi.Extensions;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});
builder.Services.AddPersistance(builder.Configuration);
builder.Services.AddApplication();
builder.Services.ConfigureCors();
builder.Services.AddAuthentication();
builder.Services.ConfigureIdentity();
builder.Services.AddAuthorization();
builder.Services.AddControllers();
builder.Services.ConfigureJWT(builder.Configuration);
builder.Services.AddJwtConfiguration(builder.Configuration);

var app = builder.Build();

DbInitializer.Initialize(app.Services);

app.ConfigureExceptionHandler();

//app.UseHttpsRedirection();

app.UseCors("CorsPolicy");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
