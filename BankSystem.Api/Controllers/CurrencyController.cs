using BankSystem.App.Services;
using Microsoft.AspNetCore.Mvc;

namespace BankSystem.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CurrencyController : ControllerBase
    {
        private readonly CurrencyService _currencyService;
        private CancellationToken _cancellationToken = default;

        public CurrencyController(CurrencyService currencyService)
        {
            _currencyService = currencyService;
        }

        [HttpGet("convert")]
        public async Task<IActionResult> Convert(decimal amount, string from, string to)
        {
            try
            {
                decimal convertedAmount = await _currencyService.ConvertCurrencyAsync(from, to, amount, _cancellationToken);
                return Ok(new { Amount = convertedAmount });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }
    }
}