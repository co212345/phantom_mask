using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using phantom_mask.Data;
using phantom_mask.Dtos;

namespace phantom_mask.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        private readonly PharmacyDbContext _context;

        public SearchController(PharmacyDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult SearchByName([FromQuery] string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                return BadRequest("Query is required.");

            query = query.Trim().ToLower();

            var pharmacies = _context.Pharmacies
                .Where(p => p.Name.ToLower().Contains(query))
                .Select(p => new PharmacySearchResultDto
                {
                    PharmacyId = p.PharmacyId,
                    Name = p.Name
                })
                .ToList();

            var masks = _context.Masks
                .Where(m => m.Name.ToLower().Contains(query))
                .Select(m => new MaskSearchResultDto
                {
                    MaskId = m.MaskId,
                    Name = m.Name,
                    Price = m.Price,
                    PharmacyId = m.PharmacyId
                })
                .ToList();

            var result = new SearchResponseDto
            {
                Pharmacies = pharmacies,
                Masks = masks
            };

            return Ok(result);
        }
    }
}
