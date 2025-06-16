using MediatR;

namespace Application;

public record GetPostByIdCommand(Guid PostId) : IRequest<PostModel>;
