using MediatR;

namespace Application;

public record GetPostsCommand(int page = 1) : IRequest<List<PostModel>>;
