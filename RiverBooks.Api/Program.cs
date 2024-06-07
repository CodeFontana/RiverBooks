using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using RiverBooks.Books.Endpoints;
using RiverBooks.Books.Extensions;
using RiverBooks.Users.Endpoints;
using RiverBooks.Users.Extensions;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
using ILoggerFactory loggerFactory = LoggerFactory.Create(options =>
{
    options.SetMinimumLevel(LogLevel.Trace);
    options.AddConsole();
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "RiverBooks API v1",
        Version = "v1",
        Description = "RiverBooks API"
    });
    //options.SwaggerDoc("v2", new OpenApiInfo
    //{
    //    Title = "RiverBooks API v2",
    //    Version = "v2",
    //    Description = "RiverBooks API"
    //});
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Specify JWT bearer token",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });
});
builder.Services.AddApiVersioning(options =>
{
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new(1, 0);
    options.ReportApiVersions = true;
}).AddMvc().AddApiExplorer(options =>
    {
        options.GroupNameFormat = "'v'VVV";
        options.SubstituteApiVersionInUrl = true;
    });
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = "Bearer";
    options.DefaultChallengeScheme = "Bearer";
}).AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new()
        {
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration.GetValue<string>("Authentication:JwtIssuer"),
            ValidateAudience = true,
            ValidAudience = builder.Configuration.GetValue<string>("Authentication:JwtAudience"),
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.ASCII.GetBytes(
                    builder.Configuration.GetValue<string>("Authentication:JwtSecurityKey"))),
            ValidateLifetime = true,
            ClockSkew = TimeSpan.FromMinutes(10)
        };
    });
builder.Services.AddAuthorization();
builder.Services.AddCors(policy =>
{
    policy.AddPolicy("OpenCorsPolicy", options =>
        options
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod());
});
builder.Services.AddBookServices(builder.Configuration, loggerFactory);
builder.Services.AddUserServices(builder.Configuration, loggerFactory);
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        // options.SwaggerEndpoint("/swagger/v2/swagger.json", "RiverBooks v2");
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "RiverBooks v1");
        options.EnableTryItOutByDefault();
        options.ConfigObject.AdditionalItems["syntaxHighlight"] = new Dictionary<string, object>
        {
            ["activated"] = false
        };
    });
}

app.UseCors("OpenCorsPolicy");
app.UseAuthentication();
app.UseAuthorization();
app.AddBookApiEndpoints();
app.AddUserApiEndpoints();
app.Run();

/*
 * The public partial class Program { } makes the Program class part of 
 * the compilation output. Without it, the implicit Program class defined 
 * by the top-level statements in .NET 6 and onwards isn't explicitly 
 * included in the compiled assembly's metadata as a type that can be 
 * referenced, which can lead to issues when trying to use it as the 
 * entry point for tests.*/
public partial class Program { }