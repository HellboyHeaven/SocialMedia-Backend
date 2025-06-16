using MediatR;

namespace Application;

public record GetCommentsByPostIdCommand(Guid PostId, int Page) : IRequest<List<CommentModel>>;
