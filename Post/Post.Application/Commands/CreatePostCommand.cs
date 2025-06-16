using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application;

public record CreatePostCommand(string Content, List<IFormFile> Medias) : IRequest<PostModel>;
