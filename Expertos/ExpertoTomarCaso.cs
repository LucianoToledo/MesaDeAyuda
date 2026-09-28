using MesaDeAyuda.Domain.Entities;
using MesaDeAyuda.Domain.Exceptions;
using MesaDeAyuda.DTOs;
using MesaDeAyuda.Persistencia;

namespace MesaDeAyuda.Expertos;

// Utilidad de desarrollo/testing, NO es el CU que se está desarrollando en este TP (ver la
// documentación del proyecto, "Endpoints de desarrollo/utilidad"). Es una versión básica y sin especificar
// formalmente de "tomar" una instancia, solo para poder probar el flujo de Asentar Resultado
// sin tener que hacer el UPDATE a mano en la base.
public interface IExpertoTomarCaso
{
    Task TomarInstancia(TomarCasoRequestDto request);
}

public class ExpertoTomarCaso : IExpertoTomarCaso
{
    private readonly IndireccionPersistencia _persistencia;

    public ExpertoTomarCaso(IndireccionPersistencia persistencia)
    {
        _persistencia = persistencia;
    }

    public async Task TomarInstancia(TomarCasoRequestDto request)
    {
        var especialistas = await _persistencia.Buscar("Especialista",
                                                        $"Legajo == {request.NroLegajoEspecialista} AND FechaHoraBaja == null");
        var especialista = especialistas.Cast<Especialista>().FirstOrDefault()
            ?? throw new BusinessException("No se ha podido encontrar el Especialista ingresado.");

        var casos = await _persistencia.Buscar("Caso", $"NumeroCaso == {request.NumeroCaso}");
        var caso = casos.Cast<Caso>().FirstOrDefault()
            ?? throw new BusinessException("No se ha podido encontrar el Caso ingresado.");

        var instanciaDisponible = caso.Instancias.FirstOrDefault(i =>
            i.EstadoActual?.Nombre == "A Asignar" && i.TipoInstancia!.SectorId == especialista.SectorId)
            ?? throw new BusinessException("No hay ninguna instancia disponible en su sector para este caso.");

        var estadosAsignada = await _persistencia.Buscar("EstadoCasoInstancia", "Nombre == \"Asignada\" AND FechaHoraBaja == null");
        var estadoAsignada = estadosAsignada.Cast<EstadoCasoInstancia>().FirstOrDefault()
            ?? throw new BusinessException("No se encontró el estado 'Asignada'.");

        instanciaDisponible.EspecialistaId = especialista.Id;
        instanciaDisponible.EstadoId = estadoAsignada.Id;
        instanciaDisponible.EstadoActual = estadoAsignada;
        instanciaDisponible.FechaHoraInicioReal = DateTime.UtcNow;

        var estadosTomado = await _persistencia.Buscar("EstadoCaso", "Nombre == \"Tomado\" AND FechaHoraBaja == null");
        var estadoTomado = estadosTomado.Cast<EstadoCaso>().FirstOrDefault()
            ?? throw new BusinessException("No se encontró el estado 'Tomado'.");

        caso.EstadoId = estadoTomado.Id;
        caso.EstadoActual = estadoTomado;

        await _persistencia.Guardar(caso);
    }
}
