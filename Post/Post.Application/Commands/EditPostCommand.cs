using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application;

public record EditPostCommand(Guid Id, string Content, string[] OldMedias, IFormFile[] NewMedias) : IRequest<PostModel>;
