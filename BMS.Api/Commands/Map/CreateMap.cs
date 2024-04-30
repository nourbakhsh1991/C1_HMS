using BMS.Services.Map.Interfaces;
using MediatR;

namespace BMS.Api.Commands.Map
{
    // Query
    public class CreateMapCommand : IRequest<string>
    {
        public Domain.Models.Map.Map Map { get; set; }
    }

    // Command Handler
    public class CreateMapCommandHandler : IRequestHandler<CreateMapCommand, string>
    {
        private readonly IMapService _mapService;

        public CreateMapCommandHandler(IMapService mapService)
        {
            _mapService = mapService;
        }

        public async Task<string> Handle(CreateMapCommand request, CancellationToken cancellationToken)
        {
            await _mapService.Insert(request.Map);
            return request.Map.Id;
        }
    }
}
