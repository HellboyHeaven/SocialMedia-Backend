using MediatR;

namespace Application;

public record RefreshTokenCommand(string Token) : IRequest<TokenModel>;
