using Microsoft.AspNetCore.Mvc;
using MouseTracker.Application.Abstractions;
using MouseTracker.Application.DTOs.Request;
using MouseTracker.Application.DTOs.Resposne;

namespace MouseTracker.Web.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class PositionsController : BaseController
    {
        private readonly IPositionsService _postionsService;

        public PositionsController(IPositionsService postionsService)
        {
            _postionsService = postionsService;
        }

        [HttpGet("{positionId}")]
        public async Task<ActionResult<PositionDto>> Get(int positionId)
        {
            var positionDto = await _postionsService.Get(positionId);
            return Ok(positionDto);
        }

        [HttpGet]
        public async Task<ActionResult<PositionDto[]>> Get(PositionFilter filter)
        {
            var positionsDto = await _postionsService.Get(filter);
            return Ok(positionsDto);
        }

        [HttpPost]
        public async Task<ActionResult<PositionDto[]>> Create([FromBody] CreatePositionDto[] models)
        {
            var positionsDto = await _postionsService.Create(models);
            return Ok(positionsDto);
        }

        [HttpDelete("{positionId}")]
        public async Task<ActionResult> Delete(int positionId)
        {
            await _postionsService.Delete(positionId);
            return Ok();
        }
    }
}
