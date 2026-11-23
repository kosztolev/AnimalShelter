using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShelterApi.Models;
using ShelterApi.Models.DTOs;

namespace ShelterApi.Controllers;

[Route("animals")]
[ApiController]
public class AnimalsController : ControllerBase
{
    private readonly AnimalshelterContext _context;

    public AnimalsController(AnimalshelterContext context)
    {
        _context = context;
    }

    [HttpPost("newAnimal")]
    public async Task<IActionResult> NewAnimal([FromBody] NewAnimalDto dto)
    {
        try
        {
            if (!await _context.Shelters.AnyAsync(s => s.Id == dto.ShelterId))
            {
                return NotFound(new { message = "A megadott menhely (ShelterId) nem létezik." });
            }

            var animal = new Animal
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                Species = dto.Species,
                Age = dto.Age,
                Weight = dto.Weight,
                RegTime = DateTime.Now,
                ShelterId = dto.ShelterId
            };

            _context.Animals.Add(animal);
            await _context.SaveChangesAsync();

            return StatusCode(201, new {message = "Állat rögzítve.", result = animal});
        }
        catch (Exception ex)
        {
            return BadRequest(new {message = $"Hiba történt: {ex.Message}"});
        }
    }

    [HttpGet("allAnimal")]
    public async Task<IActionResult> GetAllAnimal()
    {
        try
        {
            var animals = await _context.Animals.ToListAsync();
            if (animals.Count == 0)
            {
                return NotFound(new {message = "Nincs rögzített állat." });
            }

            return Ok(new {message = "Sikeres lekérdezés.", result = animals});
        }
        catch (Exception ex)
        {
            return BadRequest(new {message = $"Hiba történt: {ex.Message}"});
        }
    }

    [HttpGet("byId")]
    public async Task<IActionResult> GetById([FromQuery] Guid id)
    {
        try
        {
            var animal = await _context.Animals.FirstOrDefaultAsync(a => a.Id == id);
            if (animal == null)
            {
                return NotFound(new {message = "Nem található állat az azonosítóval"});
            }

            return Ok(new {message = "Sikeres lekérdezés.", result = animal});
        }
        catch (Exception ex)
        {
            return BadRequest(new {message = $"Hiba történt: {ex.Message}"});
        }
    }

    [HttpPut("update")]
    public async Task<IActionResult> UpdateAnimal([FromQuery] Guid id, [FromBody] UpdateAnimalDto dto)
    {
        try
        {
            var animal = await _context.Animals.FirstOrDefaultAsync(a => a.Id == id);
            if (animal == null)
            {
                return NotFound(new {message = "A módosítani kívánt állat nem található."});
            }

            var shelterExists = await _context.Shelters.AnyAsync(s => s.Id == dto.ShelterId);
            if (!shelterExists)
            {
                return NotFound(new {message = "A megadott menhely (ShelterId) nem létezik."});
            }

            animal.Name = dto.Name;
            animal.Species = dto.Species;
            animal.Age = dto.Age;
            animal.Weight = dto.Weight;
            animal.ShelterId = dto.ShelterId;

            await _context.SaveChangesAsync();

            return Ok(new {message = "Állat sikeresen módosítva.", result = animal});
        }
        catch (Exception ex)
        {
            return BadRequest(new {message = $"Hiba történt: {ex.Message}"});
        }
    }

    [HttpDelete("deleteAnimal")]
    public async Task<IActionResult> DeleteAnimal([FromQuery] Guid id)
    {
        try
        {
            var animal = await _context.Animals.FirstOrDefaultAsync(a => a.Id == id);
            if (animal == null)
            {
                return NotFound(new {message = "Nem törölhető mert nincs ilyen állat"});
            }

            _context.Animals.Remove(animal);
            await _context.SaveChangesAsync();

            return Ok(new {message = "Állat sikeresen törölve.", result = animal});
        }
        catch (Exception ex)
        {
            return BadRequest(new {message = $"Hiba történt: {ex.Message}"});
        }
    }

    [HttpGet("allAnimalsWithShelter")]
    public async Task<IActionResult> GetAllAnimalsWithShelter()
    {
        try
        {
            var result = await _context.Animals
                .Join(_context.Shelters,
                    a => a.ShelterId,
                    s => s.Id,
                    (a, s) => new
                    {
                        a.Id,
                        a.Name,
                        a.Species,
                        a.Age,
                        a.Weight,
                        a.RegTime,
                        ShelterName = s.Name
                    })
                .ToListAsync();

            if (result.Count == 0)
            {
                return NotFound(new {message = "Nincsen rögzített állat."});
            }

            return Ok(new {message = "Sikeres lekérdezés.", result});
        }
        catch (Exception ex)
        {
            return BadRequest(new {message = $"Hiba történt: {ex.Message}"});
        }
    }
}
