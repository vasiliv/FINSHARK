using api.Data;
using api.Dtos.Stock;
using api.Mappers;
using api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers
{
    [Route("api/stock")]
    [ApiController]
    public class StockController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public StockController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        //public IActionResult GetAll()
        public async Task<IActionResult> GetAll()
        {
            //var stocks = _context.Stocks.ToList()
            //    .Select(s => s.ToStockDto());
            var stocks = await _context.Stocks.ToListAsync();
            var stockDto = stocks.Select(s => s.ToStockDto());
            //return Ok(stocks);
            return Ok(stockDto);
        }
        [HttpGet("{id}")]
        //public IActionResult GetById([FromRoute] int id)
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            //var stock = _context.Stocks.Find(id);
            var stock = await _context.Stocks.FindAsync(id);

            if (stock == null)
            {
                return NotFound();
            }
            return Ok(stock.ToStockDto());
        }
        [HttpPost]
        //public IActionResult Create([FromBody] CreateStockRequestDto stockDto)
        public async Task<IActionResult> Create([FromBody] CreateStockRequestDto stockDto)
        {
            var stockModel = stockDto.ToStockFromCreateDTO();
            //_context.Stocks.Add(stockModel);
            //_context.SaveChanges();
            await _context.Stocks.AddAsync(stockModel);
            await _context.SaveChangesAsync();
            //calls GetById function, passes id, returns - stockModel.ToStockDto()
            return CreatedAtAction(nameof(GetById), new {id = stockModel.Id}, stockModel.ToStockDto());
        }
        [HttpPut]
        [Route("{id}")]
        //public IActionResult Update([FromRoute] int id, [FromBody] UpdateStockRequestDto updateDto)
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateStockRequestDto updateDto)
        {
            //var stockModel = _context.Stocks.FirstOrDefault(s => s.Id == id);
            var stockModel = await _context.Stocks.FirstOrDefaultAsync(s => s.Id == id);
            if (stockModel == null)
            {
                return NotFound();
            }

            stockModel.Symbol = updateDto.Symbol;
            stockModel.CompanyName = updateDto.CompanyName;
            stockModel.Purchase = updateDto.Purchase;
            stockModel.LastDiv = updateDto.LastDiv;
            stockModel.Industry = updateDto.Industry;
            stockModel.MarketCap = updateDto.MarketCap;

            //_context.SaveChanges();
            await _context.SaveChangesAsync();

            return Ok(stockModel.ToStockDto());
        }
        [HttpDelete]
        [Route("{id}")]
        //public IActionResult Delete([FromRoute] int id)
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            //var stockModel = _context.Stocks.FirstOrDefault(x => x.Id == id);
            var stockModel = await _context.Stocks.FirstOrDefaultAsync(x => x.Id == id);
            
            if (stockModel == null)
            {
                return NotFound();
            }

            //_context.Stocks.Remove(stockModel);
            //_context.SaveChanges();

            //Remove does not have async
            _context.Stocks.Remove(stockModel);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
