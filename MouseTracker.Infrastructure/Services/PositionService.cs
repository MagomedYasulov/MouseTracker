using MouseTracker.Application.Abstractions;
using MouseTracker.Application.DTOs.Request;
using MouseTracker.Application.DTOs.Resposne;

namespace MouseTracker.Infrastructure.Services
{
    public class PositionService : IPositionsService
    {
        public Task<PositionDto[]> Create(CreatePositionDto[] positionsDto)
        {
            throw new NotImplementedException();
        }

        public Task Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<PositionDto> Get(int id)
        {
            throw new NotImplementedException();
        }

        public Task<PositionDto[]> Get(PositionFilter filter)
        {
            throw new NotImplementedException();
        }
    }
}
