using MediatR;

namespace Application;

public record CreatePostLikeCommand(Guid AuthorId, Guid PostId) : IRequest;
