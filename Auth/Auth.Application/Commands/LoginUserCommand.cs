using MediatR;

namespace Application;

public record LoginUserCommand(string Login, string Password, string UserAgent) : IRequest<TokenModel>;
