using MesaDeAyuda.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MesaDeAyuda.Controllers;

// Utilidad de desarrollo/testing: no forma parte del CU Asentar Resultado ni del DC.
[ApiController]
[Route("api/v1/seed")]
public class SeedController : ControllerBase
{
    private readonly MesaAyudaDbContext _context;
    private readonly IHostEnvironment _environment;

    public SeedController(
        MesaAyudaDbContext context,
        IHostEnvironment environment)
    {
        _context = context;
        _environment = environment;
    }

    /// <summary>
    /// Borra la base y la recrea aplicando todas las migraciones (incluye el seed de cada una),
    /// para volver al estado inicial sin correr "dotnet ef database drop/update" a mano.
    /// Solo disponible en entorno Development. Utilidad de testing, no es parte del CU ni del DC.
    /// </summary>
    [HttpPost("reset")]
    public async Task<IActionResult> Reset()
    {
        if (!_environment.IsDevelopment())
            return NotFound();

        await _context.Database.EnsureDeletedAsync();
        await _context.Database.MigrateAsync();

        return NoContent();
    }
}
