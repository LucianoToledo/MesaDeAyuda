using MesaDeAyuda.Domain.Entities;

namespace MesaDeAyuda.Adapters;

public interface IAdaptadorNotificacionCliente
{
    bool NotificarCliente(string mensaje, Caso caso);
}