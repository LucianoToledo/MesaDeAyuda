using MesaDeAyuda.Domain.Entities;

namespace MesaDeAyuda.Repositories;

public interface ITipoCasoTipoInstanciaRepository
{
    Task<TipoCasoTipoInstancia?> ObtenerVigente(
        int tipoCasoId,
        int orden,
        DateTime fecha);
}
