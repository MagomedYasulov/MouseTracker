using MouseTracker.Application.DTOs.Request;
using MouseTracker.Application.DTOs.Resposne;

namespace MouseTracker.Application.Abstractions
{
    public interface IPositionsService
    {
        public Task<PositionDto> Get(int id);
        public Task<PositionDto[]> Get(PositionFilter filter);
        public Task<PositionDto[]> Create(CreatePositionDto[] positionsDto);
        public Task Delete(int id);
    }
}
