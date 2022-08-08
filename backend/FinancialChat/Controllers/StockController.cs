using FinancialChat.Stock;
using Microsoft.AspNetCore.Mvc;

namespace FinancialChat.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockController : Controller
    {
        private readonly IStockService _stockService;

        public StockController(IStockService stockService)
        {
            _stockService = stockService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            await _stockService.GetStockQuote("aapl.us", "coding");

            return Ok();
        }
    }
}