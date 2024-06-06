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
builder.Services.AddSwaggerGen();
builder.Services.AddBookServices(builder.Configuration, loggerFactory);
builder.Services.AddUserServices(builder.Configuration, loggerFactory);
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.EnableTryItOutByDefault();
    });
}

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