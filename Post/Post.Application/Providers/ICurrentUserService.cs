namespace Application;

public interface ICurrentUserService
{
    Guid Id { get; }
    bool Exists { get; }
    bool IsInRole(string role);
}
