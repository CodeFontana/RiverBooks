using RiverBooks.Books.Endpoints;
using RiverBooks.Books.Extensions;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddBookServices(builder.Configuration);
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.EnableTryItOutByDefault();
    });
}

app.UseHttpsRedirection();
app.AddBookApiEndpoints();
app.Run();

/*
 * The public partial class Program { } makes the Program class part of 
 * the compilation output. Without it, the implicit Program class defined 
 * by the top-level statements in .NET 6 and onwards isn't explicitly 
 * included in the compiled assembly's metadata as a type that can be 
 * referenced, which can lead to issues when trying to use it as the 
 * entry point for tests.*/
public partial class Program { }