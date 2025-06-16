using MediatR;

namespace Application;

public record DeleteCommentLikeCommand(Guid AuthorId, Guid CommentId) : IRequest;
