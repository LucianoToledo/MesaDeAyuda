using MesaDeAyuda.Domain.Entities;
using MesaDeAyuda.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace MesaDeAyuda.Repositories;

public class EstadoCasoRepository : IEstadoCasoRepository
{
    private readonly MesaAyudaDbContext _context;

    public EstadoCasoRepository(MesaAyudaDbContext context)
    {
        _context = context;
    }

    public async Task<EstadoCaso?> GetByNombreAsync(string nombre)
    {
        return await _context.EstadoCaso
            .FirstOrDefaultAsync(e => e.Nombre == nombre && e.FechaHoraBaja == null);
    }
}