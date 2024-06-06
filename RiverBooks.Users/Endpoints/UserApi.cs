using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using RiverBooks.Users.Data.Entities;
using RiverBooks.Users.Models;

namespace RiverBooks.Users.Endpoints;

public static class UserApi
{
    public static void AddUserApiEndpoints(this WebApplication app)
    {
        //app.MapGet("/api/v1/users", ReadAllAsync);
        //app.MapGet("/api/v1/users/{id:int}", ReadAsync);
        app.MapPost("/api/v1/users", CreateAsync);
        //app.MapDelete("/api/v1/users/{id:int}", DeleteAsync);
    }

    private static async Task<IResult> CreateAsync(UserManager<ApplicationUser> userManager, CreateUserRequest newUser)
    {
        try
        {
            IdentityResult result = await userManager.CreateAsync(new ApplicationUser
            {
                FirstName = newUser.FirstName,
                LastName = newUser.LastName,
                Email = newUser.Email,
                UserName = newUser.Email,
                EmailConfirmed = true
            }, newUser.Password);

            if (result.Succeeded == false)
            {
                return Results.Problem(result.Errors.First().Description, statusCode: 400);
            }

            return Results.Ok(result);
        }
        catch (Exception e)
        {
            return Results.Problem(e.Message, statusCode: 500);
        }
    }
}
