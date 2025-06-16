using MediatR;

namespace Application;

public record RegisterUserCommand(string Login, string Password) : IRequest;
