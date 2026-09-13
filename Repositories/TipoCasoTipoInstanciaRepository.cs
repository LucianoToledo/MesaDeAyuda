using MesaDeAyuda.Domain.Entities;
using MesaDeAyuda.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace MesaDeAyuda.Repositories;

public class TipoCasoTipoInstanciaRepository : ITipoCasoTipoInstanciaRepository
{
    private readonly MesaAyudaDbContext _context;

    public TipoCasoTipoInstanciaRepository(MesaAyudaDbContext context)
    {
        _context = context;
    }

    public async Task<TipoCasoTipoInstancia?> ObtenerVigente(
        int tipoCasoId,
        int orden,
        DateTime fecha)
    {
        return await _context.TipoCasoTipoInstancia
            .FirstOrDefaultAsync(t =>
                t.TipoCasoId == tipoCasoId &&
                t.Orden == orden &&
                t.FechaAlta <= fecha &&
                (t.FechaBaja == null || t.FechaBaja > fecha));
    }
}
