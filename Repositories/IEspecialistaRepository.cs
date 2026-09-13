using MesaDeAyuda.Domain.Entities;

namespace MesaDeAyuda.Repositories;

public interface IEspecialistaRepository
{
    Task<Especialista?> GetByLegajoAsync(int nroLegajo);
}
