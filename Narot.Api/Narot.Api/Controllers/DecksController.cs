using Microsoft.AspNetCore.Mvc;
using Narot.Application.Interfaces;

namespace Narot.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class DecksController : ControllerBase
    {
        private readonly INarotService _narotService;

        public DecksController(INarotService narotService)
        {
            _narotService = narotService;
        }

        [HttpGet]
        public async Task<IActionResult> getDeckNames() 
        {
            return Ok(await _narotService.GetDeckNamesAsync());
        }

        [HttpGet("{deckName}")]
        public async Task<IActionResult> getDeck(string deckName)
        {
            var deck = await _narotService.GetDeckAsync(deckName);
            return deck == null ? NotFound() : Ok(deck);
        }
    }
}
