using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using RiverBooks.Users.Data.Entities;
using RiverBooks.Users.Interfaces;
using RiverBooks.Users.Models;

namespace RiverBooks.Users.Endpoints;

public static class UserApi
{
    public static void AddUserApiEndpoints(this WebApplication app)
    {
        app.MapPost("/api/v1/users", CreateAsync);
        app.MapPost("/api/v1/users/login", LoginAsync);
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
                return Results.Problem(result.Errors.First().Description, statusCode: StatusCodes.Status400BadRequest);
            }

            return Results.Ok(result);
        }
        catch (Exception e)
        {
            return Results.Problem(e.Message, statusCode: StatusCodes.Status500InternalServerError);
        }
    }

    private static async Task<IResult> LoginAsync(UserManager<ApplicationUser> userManager,
                                                  ITokenService tokenService,
                                                  LoginUserRequest loginUser)
    {
        try
        {
            ApplicationUser? user = await userManager.FindByEmailAsync(loginUser.Email);

            if (user is null || string.IsNullOrWhiteSpace(user.UserName))
            {
                return Results.Problem("Invalid email or password.", statusCode: StatusCodes.Status401Unauthorized);
            }

            bool isValidPassword = await userManager.CheckPasswordAsync(user, loginUser.Password);

            if (isValidPassword == false)
            {
                return Results.Problem("Invalid email or password.", statusCode: StatusCodes.Status401Unauthorized);
            }

            string jwt = tokenService.CreateTokenAsync(user.UserName);

            return Results.Ok(jwt);
        }
        catch (Exception e)
        {
            return Results.Problem(e.Message, statusCode: StatusCodes.Status500InternalServerError);
        }
    }
}
