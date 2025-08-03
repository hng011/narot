using Microsoft.AspNetCore.Mvc;
using Narot.Api.DTOs;
using Narot.Application.Interfaces;

namespace Narot.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ReadingsController : ControllerBase
    {
        private readonly INarotService _narotService;

        public ReadingsController(INarotService narotService)
        {
            _narotService = narotService;
        }

        [HttpPost]
        public async Task<IActionResult> GetReadings([FromBody] ReadingRequestDto request)
        {
            if (request == null) return BadRequest("Please provide at least one question.");

            try
            {
                var reading = await _narotService.GetNarotReadingAsync(request.Questions, request.DeckName);
                return Ok(reading);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
