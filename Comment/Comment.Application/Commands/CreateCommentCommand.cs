using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application;

public record CreateCommentCommand(Guid PostId, string Content, IFormFile[] Medias) : IRequest<CommentModel>;
