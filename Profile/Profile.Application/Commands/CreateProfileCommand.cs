using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application;

public record CreateProfileCommand(string Username, string? Nickname, string Description, IFormFile? Avatar) : IRequest<Guid>;
