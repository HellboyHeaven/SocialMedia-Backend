using MediatR;

namespace Application;

public record CreateCommentLikeCommand(Guid AuthorId, Guid CommentId) : IRequest;
