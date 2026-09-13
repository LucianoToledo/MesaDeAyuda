using MesaDeAyuda.Domain.Entities;
using MesaDeAyuda.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace MesaDeAyuda.Repositories;

public class TipoTareaRepository : ITipoTareaRepository
{
    private readonly MesaAyudaDbContext _context;

    public TipoTareaRepository(MesaAyudaDbContext context)
    {
        _context = context;
    }

    public async Task<List<TipoTarea>> GetAllAsync()
    {
        return await _context.TipoTarea
            .Where(t => t.FechaHoraBaja == null)
            .ToListAsync();
    }
}
