using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application;

public record EditCommentCommand(Guid Id, string Content, string[] OldMedias, IFormFile[] NewMedias) : IRequest<CommentModel>;
