using MesaDeAyuda.Domain.Entities;
using MesaDeAyuda.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace MesaDeAyuda.Repositories;

public class EspecialistaRepository : IEspecialistaRepository
{
    private readonly MesaAyudaDbContext _context;

    public EspecialistaRepository(MesaAyudaDbContext context)
    {
        _context = context;
    }

    public async Task<Especialista?> GetByLegajoAsync(int nroLegajo)
    {
        return await _context.Especialista
            .FirstOrDefaultAsync(e => e.Legajo == nroLegajo && e.FechaHoraBaja == null);
    }
}
