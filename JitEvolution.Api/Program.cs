using JitEvolution.Api.Middlewares;
using JitEvolution.BusinessObjects;
using JitEvolution.Config;
using JitEvolution.Data;
using JitEvolution.Neo4J.Data;
using JitEvolution.Notifications;
using JitEvolution.Services;
using JitEvolution.Services.Identity;
using JitEvolution.SignalR.Hubs;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;
using System.Text;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.RegisterRepositories();
builder.Services.RegisterDatabases(builder.Configuration);
builder.Services.RegisterIdentity();
builder.Services.RegisterBusinessObjects();
builder.Services.RegisterServices();
builder.Services.RegisterNeo4JServices();

builder.Services.AddMediatR(
    Assembly.GetExecutingAssembly(),
    Assembly.GetAssembly(typeof(JitEvolutionHub)),
    Assembly.GetAssembly(typeof(JitEvolution.Services.ServiceCollectionExtensions)),
    Assembly.GetAssembly(typeof(ProjectAdded)));

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

    cfg.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            var accessToken = context.Request.Query["access_token"];
            var pathString = context.HttpContext.Request.Path.ToString();

            if (!string.IsNullOrEmpty(accessToken) &&
                (pathString.StartsWith("/hub/") || pathString.StartsWith("/swagger/")))
            {
                context.Token = accessToken;
            }

            return Task.CompletedTask;
        }
    };
}).AddScheme<ApiKeyAuthenticationSchemeOptions, ApiKeyAuthenticationHandler>(ApiKeyAuthenticationSchemeOptions.DefaultScheme, opt => { });

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSignalR();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("Development");

//app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<CorsMiddleware>();
app.UseMiddleware<CurrentUserMiddleWare>();
app.UseMiddleware<ErrorHandlingMiddleware>();

app.MapControllers();


app.UseEndpoints(endpoints =>
{
    endpoints.MapHub<JitEvolutionHub>("/hub");
});

app.Run();
