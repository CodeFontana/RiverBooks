namespace RiverBooks.Users.Models;

public record CreateUserRequest(string FirstName, string LastName, string Email, string Password);
