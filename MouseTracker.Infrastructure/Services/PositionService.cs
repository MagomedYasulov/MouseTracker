using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using MouseTracker.Application.Abstractions;
using MouseTracker.Application.DTOs.Request;
using MouseTracker.Application.DTOs.Resposne;
using MouseTracker.Domain.Data;
using MouseTracker.Domain.Data.Entites;
using MouseTracker.Domain.Exceptions;
using System.Linq.Expressions;

namespace MouseTracker.Infrastructure.Services
{
    public class PositionService : IPositionsService
    {
        private readonly IMapper _mapper;
        private readonly ApplicationContext _dbContext;

        public PositionService(
            IMapper mapper, 
            ApplicationContext dbContext)
        {
            _mapper = mapper;
            _dbContext = dbContext;
        }

        public async Task<PositionDto> Get(int id)
        {
            var position = await _dbContext.Positions.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
            if (position == null)
                throw new ServiceException("Position Not Found", $"Position with id {id} not found.", StatusCodes.Status404NotFound);

            return _mapper.Map<PositionDto>(position);
        }

        public async Task<PositionDto[]> Get(PositionFilter filter)
        {
            var isStartTimeNull = filter.StartTime == null;
            var isEndTimeNull = filter.EndTime == null;

            Expression<Func<Position, bool>> exp = r => ((isStartTimeNull || r.MoveTime >= filter.StartTime) &&
                                                         (isEndTimeNull || r.MoveTime <= filter.EndTime));

            var positions = await _dbContext.Positions.AsNoTracking().Where(exp).ToArrayAsync();
            return _mapper.Map<PositionDto[]>(positions);
        }

        public async Task<PositionDto[]> Create(CreatePositionDto[] models)
        {
            var positions = _mapper.Map<Position[]>(models);
            await _dbContext.Positions.AddRangeAsync(positions);
            await _dbContext.SaveChangesAsync();

            var positionsDto = _mapper.Map<PositionDto[]>(positions);
            return positionsDto;
        }
 
        public async Task Delete(int id)
        {
            var position = await _dbContext.Positions.FirstOrDefaultAsync(m => m.Id == id);
            if (position == null)
                throw new ServiceException("Position Not Found", $"Position with id {id} not found.", StatusCodes.Status404NotFound);

            _dbContext.Positions.Remove(position);
            await _dbContext.SaveChangesAsync();
        }
    }
}
