using MesaDeAyuda.DTOs;

namespace MesaDeAyuda.Adapters;

public interface IAdaptadorNotificacionCliente
{
    bool Notificar(DTONotificacionCliente dtoNotificacion);
}
