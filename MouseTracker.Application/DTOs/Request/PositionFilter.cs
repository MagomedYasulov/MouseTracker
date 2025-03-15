using Microsoft.AspNetCore.Mvc;

namespace MouseTracker.Application.DTOs.Request
{
    public class PositionFilter
    {
        [FromQuery]
        public DateTime? StartTime { get; set; }
        [FromQuery]
        public DateTime? EndTime { get; set; }
    }
}
