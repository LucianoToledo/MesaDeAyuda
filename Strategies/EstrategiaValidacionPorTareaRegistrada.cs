using MesaDeAyuda.Domain.Entities;
using MesaDeAyuda.DTOs;

namespace MesaDeAyuda.Strategies;

public class EstrategiaValidacionPorTareaRegistrada : IEstrategiaValidacionCierre
{
    private readonly ILogger<EstrategiaValidacionPorTareaRegistrada> _logger;

    public EstrategiaValidacionPorTareaRegistrada(ILogger<EstrategiaValidacionPorTareaRegistrada> logger)
    {
        _logger = logger;
    }

    public DTOResultadoValidacion ValidarCierre(CasoInstancia instancia)
    {
        _logger.LogInformation("Ejecutando estrategia {Estrategia}", nameof(EstrategiaValidacionPorTareaRegistrada));

        if (!instancia.Tareas.Any())
            return new DTOResultadoValidacion(false, "Debe registrar al menos una tarea antes de poder asentar el resultado de la instancia.");

        return new DTOResultadoValidacion(true, string.Empty);
    }
}