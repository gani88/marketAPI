using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
[ApiController]
public class BarangController : ControllerBase
{
    private readonly WarehouseContext warehouseContext;

    public BarangController(WarehouseContext context)
    {
        warehouseContext = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Barang>>> GetBarangs()
    {
        return await warehouseContext.Barangs.Include(b => b.Gudang).ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Barang>> GetBarang(int id)
    {
        var barang = await warehouseContext.Barangs.Include(b => b.Gudang).FirstOrDefaultAsync(b => b.Id == id);
        if (barang == null)
        {
            return NotFound();
        }
        return barang;
    }

    [HttpGet("monitoring")]
    public async Task<ActionResult<IEnumerable<Barang>>> GetFilteredBarangs([FromQuery] string namaGudang, [FromQuery] DateTime? expiredDate)
    {
        var query = warehouseContext.Barangs.Include(b => b.Gudang).AsQueryable();

        if (!string.IsNullOrEmpty(namaGudang))
        {
            query = query.Where(b => b.Gudang.NamaGudang == namaGudang);
        }

        if (expiredDate.HasValue)
        {
            // Ensure the expiredDate is treated as UTC
            var utcExpiredDate = DateTime.SpecifyKind(expiredDate.Value, DateTimeKind.Utc);
            query = query.Where(b => b.ExpiredDate >= utcExpiredDate && b.ExpiredDate < utcExpiredDate.AddDays(1));
        }

        var result = await query.ToListAsync();
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<Barang>> PostBarang(Barang barang)
    {
        warehouseContext.Barangs.Add(barang);
        await warehouseContext.SaveChangesAsync();
        return CreatedAtAction(nameof(GetBarang), new { id = barang.Id }, barang);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutBarang(int id, Barang barang)
    {
        if (id != barang.Id)
        {
            return BadRequest();
        }

        warehouseContext.Entry(barang).State = EntityState.Modified;
        try
        {
            await warehouseContext.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!warehouseContext.Barangs.Any(e => e.Id == id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBarang(int id)
    {
        var barang = await warehouseContext.Barangs.FindAsync(id);
        if (barang == null)
        {
            return NotFound();
        }

        warehouseContext.Barangs.Remove(barang);
        await warehouseContext.SaveChangesAsync();
        
        return Ok("Barang deleted successfully");
    }
}