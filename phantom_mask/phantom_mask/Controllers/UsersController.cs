using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using phantom_mask.Data;
using phantom_mask.Dtos;
using System.Data;

namespace phantom_mask.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly PharmacyDbContext _context;

        public UsersController(PharmacyDbContext context)
        {
            _context = context;
        }

        [HttpGet("top-by-amount")]
        public IActionResult GetTopUsersByTrabsactionAmount(
            [FromQuery] DateTime start,
            [FromQuery] DateTime end,
            [FromQuery] int top =5)
        {
            var topUsers = _context.Users
                .Select(u => new
                {
                    u.UserId,
                    u.Name,
                    TotalAmount = u.PurchaseHistories
                        .Where(p=>p.TransactionDate >= start&&p.TransactionDate<= end)
                        .Sum(p=> p.TransactionAmount)
                })
                .OrderByDescending(u=>u.TotalAmount)
                .Take(top)
                .ToList();
            var result = topUsers
                .Select(u=>new TopUserDto
                {
                    UserId = u.UserId,
                    Name = u.Name,
                    TotalAmount = u.TotalAmount,
                })
                .ToList();
            return Ok(result);
        }

        [HttpGet("transaction-summary")]
        public IActionResult GetTransactionSummary(
            [FromQuery] DateTime start,
            [FromQuery] DateTime end)
        {
            var summary = _context.PurchaseHistories
                .Where(p => p.TransactionDate >= start && p.TransactionDate < end)
                .GroupBy(p => 1)
                .Select(g => new TransactionSummaryDto
                {
                    TotalQuantity = g.Count(),
                    TotalAmount = g.Sum(p => p.TransactionAmount)
                })
                .FirstOrDefault() ?? new TransactionSummaryDto();
            return Ok(summary);
        }
    }
}
