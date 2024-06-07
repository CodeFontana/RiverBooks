namespace RiverBooks.Users.Interfaces;

public interface ITokenService
{
    string CreateTokenAsync(string username);
}