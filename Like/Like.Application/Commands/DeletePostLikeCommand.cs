using MediatR;

namespace Application;

public record DeletePostLikeCommand(Guid AuthorId, Guid PostId) : IRequest;
