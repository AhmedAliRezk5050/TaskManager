namespace Core.Interfaces.Services;

public interface IUserContextService
{
    string? GetUserId();
    string? GetUserEmail();
    string? GetUserName();
}
