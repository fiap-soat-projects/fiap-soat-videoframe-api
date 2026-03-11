using Adapter;
using Api.Authentication.Factories;
using Api.Middlewares;
using Domain;
using Infrastructure;
using Infrastructure.Providers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using System.Text.Json;
using System.Text.Json.Serialization;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

ConfigureDependencies(builder.Services);

var app = builder.Build();

app.UseMiddleware<ErrorHandlingMiddleware>();
app.UseMiddleware<FileTypeValidationMiddleware>();


app.MapOpenApi();
app.MapScalarApiReference();
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();


static void ConfigureDependencies(IServiceCollection services)
{
    AddJwtAuth(services);
    services.AddHttpContextAccessor();
    //services.AddScoped<IUserContext, UserContextMocked>();
    services.AddScoped(UserContextFactory.Create);

    services
        .InjectDomainDependencies()
        .InjectInfrastructureDependencies()
        .InjectAdapterDependencies();

    services
        .AddEndpointsApiExplorer()
        .AddControllers()
        .AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
            options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
        });
}

static void AddJwtAuth(IServiceCollection services)
{
    var region = StaticEnvironmentVariableProvider.CognitoRegion;
    var userPoolId = StaticEnvironmentVariableProvider.CognitoUserPoolId;

    var cognitoUrl = $"https://cognito-idp.{region}.amazonaws.com/{userPoolId}";

    services
        .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.Authority = cognitoUrl;

            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false
            };
        });
}