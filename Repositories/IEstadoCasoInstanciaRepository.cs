using MesaDeAyuda.Domain.Entities;

namespace MesaDeAyuda.Repositories;

public interface IEstadoCasoInstanciaRepository
{
    Task<EstadoCasoInstancia?> GetByNombreAsync(string nombre);
}
