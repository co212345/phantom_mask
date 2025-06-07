using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using phantom_mask.Data;
using phantom_mask.Dtos;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace phantom_mask.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PharmaciesController : ControllerBase
    {
        private readonly PharmacyDbContext _context;

        public PharmaciesController(PharmacyDbContext context)
        {
            _context = context;
        }

        [HttpGet("open")]
        public IActionResult GetOpenPharmacies([FromQuery] int day, [FromQuery] string time)
        {
            if (day < 0 || day > 6)
            {
                return BadRequest("Invalid day format. Use 0-6");
            }
            if (!TimeSpan.TryParse(time, out var targetTime))
            {
                return BadRequest("Invalid time format. Use HH:mm");
            }

            var pharmacies = _context.Pharmacies
                .Include(p => p.OpeningHours)
                .Where(p => p.OpeningHours.Any(h =>
                    h.Day == (DayOfWeek)day &&
                    h.StartTime <= targetTime &&
                    h.EndTime >= targetTime
                ))
                .Select(p => new OpenPharmacyDto
                {
                    PharmacyId = p.PharmacyId,
                    Name =  p.Name,
                })
                .ToList();

            return Ok(pharmacies);
        }

        [HttpGet("{pharmacyId}/masks")]
        public IActionResult GetMasksByPharmacy(int pharmacyId)
        {
            var query = _context.Masks
                .Where(m => m.PharmacyId == pharmacyId);

            var result = query
                .Select(m => new PharmacyMaskDto
                {
                    Name = m.Name,
                    Price = m.Price
                })
                .ToList();

            return Ok(result);
        }

        [HttpGet("mask-count")]
        public IActionResult FilterPharmaciesByMaskCount(
            [FromQuery] float minPrice,
            [FromQuery] float maxPrice,
            [FromQuery] int x,
            [FromQuery] string type = "more")
        {
            var pharmacies = _context.Pharmacies
                .Select(p => new
                {
                    p.PharmacyId,
                    p.Name,
                    MaskCount = p.Masks
                        .Where(m => m.Price >= minPrice && m.Price <= maxPrice)
                        .Count()
                });

            // 過濾：比 x 多 or 少
            pharmacies = type.ToLower() switch
            {
                "less" => pharmacies.Where(p => p.MaskCount < x),
                _ => pharmacies.Where(p => p.MaskCount > x),
            };

            var result = pharmacies
                .Select(p => new FilteredPharmacyDto
                {
                    PharmacyId = p.PharmacyId,
                    Name = p.Name,
                    MaskCount = p.MaskCount
                })
                .ToList();

            return Ok(result);
        }


    }
}
