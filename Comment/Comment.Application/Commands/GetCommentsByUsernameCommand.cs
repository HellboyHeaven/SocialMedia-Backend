using MediatR;

namespace Application;

public record GetCommentsByUsernameCommand(string Username, int Page = 1) : IRequest<List<CommentWithPostIdModel>>;
