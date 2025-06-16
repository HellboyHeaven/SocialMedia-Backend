using MediatR;

namespace Application;

public record DeletePostCommand(Guid Id) : IRequest;
