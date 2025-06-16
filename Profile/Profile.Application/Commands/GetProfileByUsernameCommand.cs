using MediatR;

namespace Application;

public record GetProfileByUsernameCommand(string Username) : IRequest<ProfileModel>;
