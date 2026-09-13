using MesaDeAyuda.Domain.Entities;
using MesaDeAyuda.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace MesaDeAyuda.Repositories;

public class CasoRepository : ICasoRepository
{
    private readonly MesaAyudaDbContext _context;

    public CasoRepository(MesaAyudaDbContext context)
    {
        _context = context;
    }

    public async Task<Caso?> GetByNumeroAsync(int numeroCaso)
    {
        return await _context.Caso
            .Include(c => c.EstadoActual)
            .Include(c => c.Instancias)
                .ThenInclude(i => i.EstadoActual)
            .Include(c => c.Instancias)
                .ThenInclude(i => i.Especialista)
            .Include(c => c.Instancias)
                .ThenInclude(i => i.Tareas)
            .Include(c => c.Instancias)
                .ThenInclude(i => i.TipoInstancia)
            .FirstOrDefaultAsync(c => c.NumeroCaso == numeroCaso);
    }

    public async Task UpdateAsync(Caso caso)
    {
        _context.Caso.Update(caso);
        await _context.SaveChangesAsync();
    }
}
