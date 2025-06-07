using Azure.Messaging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using phantom_mask.Data;
using phantom_mask.Dtos;
using phantom_mask.Models;

namespace phantom_mask.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private readonly PharmacyDbContext _context;
        public TransactionsController(PharmacyDbContext context)
        {
            _context   = context;
        }
        [HttpPost("purchase")]
        public async Task<IActionResult> PurchaseMasks( PurchaseRequestDto request)
        {
            var user = await _context.Users
                .Include(u => u.PurchaseHistories)
                .FirstOrDefaultAsync(u => u.UserId == request.UserId);

            if (user == null)
                return NotFound("User not found.");

            float totalCost = request.Items.Sum(i => i.TransactionAmount);

            if (user.CashBalance < totalCost)
                return BadRequest("Insufficient user balance.");

            foreach (var item in request.Items)
            {
                var pharmacy = await _context.Pharmacies
                    .FirstOrDefaultAsync(p => p.Name == item.PharmacyName);

                if (pharmacy == null)
                    return BadRequest($"Pharmacy '{item.PharmacyName}' not found.");

                var purchase = new PurchaseHistory
                {
                    PharmacyName = item.PharmacyName,
                    MaskName = item.MaskName,
                    TransactionAmount = item.TransactionAmount,
                    TransactionDate = DateTime.UtcNow,
                    UserId = user.UserId
                };

                
                user.CashBalance -= item.TransactionAmount;
                pharmacy.CashBalance += item.TransactionAmount;

                _context.Entry(pharmacy).State = EntityState.Modified;

                _context.PurchaseHistories.Add(purchase);
            }

            await _context.SaveChangesAsync();

            return Ok(new { message = "Purchase completed successfully." });
        }
    }
}
