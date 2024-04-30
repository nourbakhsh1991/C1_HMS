using BMS.Domain.Geometry.Insert;
using BMS.Services.Map.Interfaces;
using MediatR;

namespace BMS.Api.Commands.Map
{
    // Query
    public class UpdateEntityCommand : IRequest<string>
    {
        public Domain.Models.Map.Entity Entity { get; set; }
    }

    // Command Handler
    public class UpdateEntityCommandHandler : IRequestHandler<UpdateEntityCommand, string>
    {
        private readonly IGeometryService _geometryService;

        public UpdateEntityCommandHandler(IGeometryService geometryService)
        {
            _geometryService = geometryService;
        }

        public async Task<string> Handle(UpdateEntityCommand request, CancellationToken cancellationToken)
        {
            var entity = _geometryService.GetById(request.Entity.Id);
            entity.StrokeColor = request.Entity.StrokeColor;
            entity.StrokeWidth = request.Entity.StrokeWidth;
            entity.Fill = request.Entity.Fill;
            //if (entity.Data is InsertEntity data)
            //{
            //    data.StrokeColor = request.Entity.StrokeColor;
            //    data.StrokeWidth = request.Entity.StrokeWidth;
            //}
            entity.Interactive = request.Entity.Interactive;
            entity.OnFrameRequested = request.Entity.OnFrameRequested;
            entity.OnClick = request.Entity.OnClick;
            entity.OnPointerDown = request.Entity.OnPointerDown;
            entity.OnPointerMove = request.Entity.OnPointerMove;
            entity.OnPointerOut = request.Entity.OnPointerOut;
            entity.OnPointerOver = request.Entity.OnPointerOver;
            entity.OnPointerUp = request.Entity.OnPointerUp;
            await _geometryService.Update(entity);
            return request.Entity.Id;
        }
    }
}
