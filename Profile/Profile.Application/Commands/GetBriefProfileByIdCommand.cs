using MediatR;

namespace Application;

public record GetBriefProfileByIdCommand(Guid UserId) : IRequest<BriefProfileModel>;
