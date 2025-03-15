using AutoMapper;
using MouseTracker.Application.DTOs.Request;
using MouseTracker.Application.DTOs.Resposne;
using MouseTracker.Domain.Data.Entites;

namespace MouseTracker.Application.Models
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<CreatePositionDto, Position>();
            CreateMap<Position, PositionDto>();
        }
    }
}
