using JitEvolution.Api.Middlewares;
using JitEvolution.BusinessObjects;
using JitEvolution.Config;
using JitEvolution.Data;
using JitEvolution.Services;
using JitEvolution.Services.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.RegisterRepositories();
builder.Services.RegisterDatabases(builder.Configuration);
builder.Services.RegisterIdentity();
builder.Services.RegisterBusinessObjects();
builder.Services.RegisterServices();

builder.Services.AddOptions();
builder.Services.Configure<Configuration>(builder.Configuration.GetSection("JitEvolution"));

var config = builder.Configuration.GetSection("JitEvolution:Jwt").Get<Jwt>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(cfg =>
{
    cfg.TokenValidationParameters = new TokenValidationParameters
    {
        ValidAudience = config.Audience,
        ValidIssuer = config.Issuer,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.Secret)),
        RequireExpirationTime = true,
        ValidateLifetime = true
    };
}).AddScheme<ApiKeyAuthenticationSchemeOptions, ApiKeyAuthenticationHandler>(ApiKeyAuthenticationSchemeOptions.DefaultScheme, opt => { });

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<CurrentUserMiddleWare>();
app.UseMiddleware<ErrorHandlingMiddleware>();

app.MapControllers();

app.Run();
