using MediatR;

namespace Application;

public record GetMyBriefProfileCommand() : IRequest<BriefProfileModel>;
