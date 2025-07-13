using Inventory.API.Interfaces;
using Inventory.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.API.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class SalesController : ControllerBase
    {
        private readonly ISaleService _saleService;

        public SalesController(ISaleService saleService)
        {
            _saleService = saleService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllSales()
        {
            var sales = await _saleService.GetAllSalesAsync();
            return Ok(sales);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSale(int id)
        {
            var sale = await _saleService.GetSaleByIdAsync(id);
            if (sale == null)
                return NotFound();
            return Ok(sale);
        }

        [HttpPost]
        public async Task<IActionResult> CreateSale([FromBody] Sale sale)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var createdSale = await _saleService.CreateSaleAsync(sale);
                return CreatedAtAction(nameof(GetSale), new { id = createdSale.SaleId }, createdSale);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("report")]
        public async Task<IActionResult> GetSalesReport([FromQuery] DateTime fromDate, [FromQuery] DateTime toDate)
        {
            if (fromDate > toDate)
                return BadRequest("From date cannot be after to date");

            var report = await _saleService.GetSalesReportAsync(fromDate, toDate);
            return Ok(report);
        }

        [HttpGet("customer/{customerId}")]
        public async Task<IActionResult> GetCustomerSales(int customerId)
        {
            var sales = await _saleService.GetSalesByCustomerAsync(customerId);
            return Ok(sales);
        }
    }
}
