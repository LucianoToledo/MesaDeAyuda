using MesaDeAyuda.Domain.Entities;

namespace MesaDeAyuda.Strategies;

public interface IEstrategiaValidacionCierre
{
    // Lanza BusinessException con un mensaje propio si la instancia no cumple lo exigido.
    void ValidarCierre(CasoInstancia instancia);
}