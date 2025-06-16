using MediatR;

namespace Application;

public record RevokeTokenCommand(Guid UserId, string UserAgent) : IRequest;
