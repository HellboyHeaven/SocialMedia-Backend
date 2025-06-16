using MediatR;

namespace Application;

public record GetPostsByUsernameCommand(string Username, int Page = 1) : IRequest<List<PostModel>>;
