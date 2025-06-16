using MediatR;

namespace Application;

public record GetCommentByIdCommand(Guid Id) : IRequest<CommentModel>;
