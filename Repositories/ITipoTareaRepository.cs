using MesaDeAyuda.Domain.Entities;

namespace MesaDeAyuda.Repositories;

public interface ITipoTareaRepository
{
    Task<List<TipoTarea>> GetAllAsync();
}
