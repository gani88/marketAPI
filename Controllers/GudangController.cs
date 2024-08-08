using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


[Route("api/[controller]")]
[ApiController]
public class GudangController : ControllerBase
{
    private readonly WarehouseContext warehouseContext;

    public GudangController(WarehouseContext context)
    {
        warehouseContext = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Gudang>>> GetGudangs()
    {
        return await warehouseContext.Gudangs.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Gudang>> GetGudang(int id)
    {
        var gudang = await warehouseContext.Gudangs.FindAsync(id);
        if (gudang == null)
        {
            return NotFound();
        }

        return gudang;
    }

    [HttpPost]
    public async Task<ActionResult<Gudang>> PostGudang(Gudang gudang)
    {
        warehouseContext.Gudangs.Add(gudang);
        await warehouseContext.SaveChangesAsync();
        return CreatedAtAction(nameof(GetGudang), new { id = gudang.Id }, gudang);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutGudang(int id, Gudang gudang)
    {
        if (id != gudang.Id)
        {
            return BadRequest();
        }

        warehouseContext.Entry(gudang).State = EntityState.Modified;
        try 
        {
            await warehouseContext.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!warehouseContext.Gudangs.Any(exception => exception.Id == id))
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
    public async Task<IActionResult> DeleteGudang(int id)
    {
        var gudang = await warehouseContext.Gudangs.FindAsync(id);
        if (gudang == null)
        {
            return NotFound();
        }

        warehouseContext.Gudangs.Remove(gudang);
        await warehouseContext.SaveChangesAsync();

        return Ok("Gudang deleted successfully");
    }
}

/**
This GudangController class is a part of an ASP.NET Core web API. 
It defines several HTTP methods that handle CRUD operations on Gudang objects.

- GetGudangs
    This method retrieves all Gudang objects from the database and returns them as an IEnumerable.
- GetGudang
    This method retrieves a Gudang object by its ID from the database and returns it. 
    If the object is not found, it returns a NotFound response.
- PostGudang
    This method creates a new Gudang object and adds it to the database.
    It then returns a CreatedAtAction response with the newly created Gudang object.
- PutGudang
    This method updates an existing Gudang object in the database.
    It first checks if the ID in the request body matches the ID in the URL.
    If they don't match, it returns a BadRequest response. 
    It then updates the state of the Gudang object in the database and saves the changes. 
    
    If a DbUpdateConcurrencyException is thrown, it checks if the Gudang object still exists in the database. 
    If it doesn't, it returns a NotFound response.
- DeleteGudang
    This method deletes a Gudang object from the database.
    It first checks if the Gudang object exists in the database.
    If it doesn't, it returns a NotFound response.
    It then removes the Gudang object from the database and saves the changes.

The WarehouseContext is injected into the constructor of the GudangController and is used to interact with the database.
**/