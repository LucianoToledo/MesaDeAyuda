using MesaDeAyuda.Domain.Entities;
using MesaDeAyuda.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace MesaDeAyuda.Repositories;

public class EstadoCasoInstanciaRepository : IEstadoCasoInstanciaRepository
{
    private readonly MesaAyudaDbContext _context;

    public EstadoCasoInstanciaRepository(MesaAyudaDbContext context)
    {
        _context = context;
    }

    public async Task<EstadoCasoInstancia?> GetByNombreAsync(string nombre)
    {
        return await _context.EstadoCasoInstancia
            .FirstOrDefaultAsync(e => e.Nombre == nombre && e.FechaHoraBaja == null);
    }
}