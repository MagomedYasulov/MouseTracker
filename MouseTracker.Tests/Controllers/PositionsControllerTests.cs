using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MouseTracker.Application.Abstractions;
using MouseTracker.Application.DTOs.Request;
using MouseTracker.Application.DTOs.Resposne;
using MouseTracker.Application.Models;
using MouseTracker.Domain.Data;
using MouseTracker.Domain.Exceptions;
using MouseTracker.Infrastructure.Services;
using MouseTracker.Web.Controllers;

namespace MouseTracker.Tests.Controllers
{
    public class PositionsControllerTests
    {
        private readonly IMapper _mapper;
        private readonly ApplicationContext _dbContext;
        private readonly IPositionsService _positionsService;

        public PositionsControllerTests()
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationContext>();
            optionsBuilder.UseInMemoryDatabase(databaseName: "PositionsControllerTests");
            var mapperConfig = new MapperConfiguration(cfg => cfg.AddProfile(new AutoMapperProfile()));

            _dbContext = new ApplicationContext(optionsBuilder.Options);

            _mapper = new Mapper(mapperConfig);

            _positionsService = new PositionService(_mapper, _dbContext);
            Common.SeedData(_dbContext);
        }

        #region Get Tests

        [Fact]
        public async Task Get_Positions_By_Id()
        {
            // Arrange
            var positionsController = new PositionsController(_positionsService);

            // Act
            var result = await positionsController.Get(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var positionDto = Assert.IsType<PositionDto>(okResult.Value);
            Assert.NotNull(positionDto);
            Assert.Equal(1, positionDto.Id);
        }


        [Fact]
        public async Task Get_Not_Exist_Positions_By_Id()
        {
            // Arrange
            var controller = new PositionsController(_positionsService);

            // Act
            Func<Task> act = async () => await controller.Get(1988739);

            // Assert
            var exception = await Assert.ThrowsAsync<ServiceException>(act);

            Assert.Equal("Position Not Found", exception.Title);
            Assert.Equal($"Position with id 1988739 not found.", exception.Message);
            Assert.Equal(StatusCodes.Status404NotFound, exception.StatusCode);
        }

        [Fact]
        public async Task Get_All_Positions()
        {
            // Arrange
            var controller = new PositionsController(_positionsService);
            var filter = new PositionFilter();

            // Act
            var result = await controller.Get(filter);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var positionsDto = Assert.IsType<PositionDto[]>(okResult.Value);
            Assert.Equal(200, positionsDto.Length);
        }


        [Fact]
        public async Task Get_Positions_By_Filter()
        {
            // Arrange
            var controller = new PositionsController(_positionsService);
            var filter = new PositionFilter() { StartTime = DateTime.UtcNow - TimeSpan.FromSeconds(200), EndTime = DateTime.UtcNow + TimeSpan.FromSeconds(200)};
            var all = await _dbContext.Positions.ToArrayAsync();

            // Act
            var result = await controller.Get(filter);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var positionsDto = Assert.IsType<PositionDto[]>(okResult.Value);

            Assert.DoesNotContain(positionsDto, p => p.MoveTime < filter.StartTime!.Value && p.MoveTime > filter.EndTime!.Value);
        }

        #endregion

        #region Create Tests

        [Fact]
        public async Task Crete_Positions()
        {
            // Arrange
            var positionsController = new PositionsController(_positionsService);
            var models = new CreatePositionDto[10];
            var random = new Random();
            for(int i=0; i< 10;i++)
            {
                models[i] = new CreatePositionDto()
                {
                    X = random.Next(0, 1980),
                    Y = random.Next(0, 1080),
                    MoveTime = DateTime.UtcNow - TimeSpan.FromSeconds(100) + TimeSpan.FromSeconds(i)
                };
            }
            // Act
            var result = await positionsController.Create(models);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var positionsDto = Assert.IsType<PositionDto[]>(okResult.Value);
            Assert.Equal(10, positionsDto.Length);
            
            for(int i=0;i< 10;i++)
            {
                Assert.NotEqual(0, positionsDto[i].Id);
                Assert.Equal(models[i].X, positionsDto[i].X);
                Assert.Equal(models[i].Y, positionsDto[i].Y);
                Assert.Equal(models[i].MoveTime, positionsDto[i].MoveTime);

                var position = await _dbContext.Positions.AsNoTracking().FirstOrDefaultAsync(p => p.Id == positionsDto[i].Id);
                Assert.NotNull(position);
                Assert.Equal(models[i].X, position.X);
                Assert.Equal(models[i].Y, position.Y);
                Assert.Equal(models[i].MoveTime, position.MoveTime);
            }
        }

        #endregion

        #region Delete tests

        [Fact]
        public async Task Delete_Not_Exist_Positions()
        {
            // Arrange
            var controller = new PositionsController(_positionsService);

            // Act
            Func<Task> act = async () => await controller.Delete(1988739);

            // Assert
            var exception = await Assert.ThrowsAsync<ServiceException>(act);

            Assert.Equal("Position Not Found", exception.Title);
            Assert.Equal($"Position with id 1988739 not found.", exception.Message);
            Assert.Equal(StatusCodes.Status404NotFound, exception.StatusCode);
        }

        [Fact]
        public async Task Delete_Positions()
        {
            // Arrange
            var controller = new PositionsController(_positionsService);

            // Act
            var result = await controller.Delete(1);

            // Assert
            Assert.IsType<OkResult>(result);
            Assert.False(_dbContext.Positions.Any(p => p.Id == 1));
        }

        #endregion
    }
}
