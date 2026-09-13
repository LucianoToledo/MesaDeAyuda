using MesaDeAyuda.Domain.Entities;

namespace MesaDeAyuda.Repositories;

public interface ICasoRepository
{
    Task<Caso?> GetByNumeroAsync(int numeroCaso);
    Task UpdateAsync(Caso caso);
}
