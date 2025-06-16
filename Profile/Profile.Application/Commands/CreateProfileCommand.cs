using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application;

public record CreateProfileCommand(Guid UserId, string Username, string? Nickname, string Description, IFormFile? Avatar) : IRequest<Guid>;
