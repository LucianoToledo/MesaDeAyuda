using MesaDeAyuda.Domain.Entities;

namespace MesaDeAyuda.Repositories;

public interface IEstadoCasoRepository
{
    Task<EstadoCaso?> GetByNombreAsync(string nombre);
}
