using MediatR;

namespace Application;

public record DeleteCommentCommand(Guid Id) : IRequest;
