using BMS.Services.Map.Interfaces;
using MediatR;

namespace BMS.Api.Commands.Map
{
    // Query
    public class CreateLayerCommand : IRequest<string>
    {
        public Domain.Models.Map.Layer Layer { get; set; }
    }

    // Command Handler
    public class CreateLayerCommandHandler : IRequestHandler<CreateLayerCommand, string>
    {
        private readonly ILayerService _layerService;

        public CreateLayerCommandHandler(ILayerService layerService)
        {
            _layerService = layerService;
        }

        public async Task<string> Handle(CreateLayerCommand request, CancellationToken cancellationToken)
        {
            await _layerService.Insert(request.Layer);
            return request.Layer.Id;
        }
    }
}
