using MesaDeAyuda.Domain.Entities;
using MesaDeAyuda.Domain.Exceptions;
using MesaDeAyuda.DTOs;
using MesaDeAyuda.Persistencia;

namespace MesaDeAyuda.Expertos;

// Utilidad de desarrollo/testing, NO es el CU que se está desarrollando en este TP (ver la
// documentación del proyecto, "Endpoints de desarrollo/utilidad"). "Registrar Tarea" es un CU propio del diagrama
// de casos de uso; acá se implementa una versión básica y sin especificar formalmente, solo para
// poder probar de punta a punta la Strategy EstrategiaValidacionPorTareaRegistrada.
public interface IExpertoTarea
{
    Task RegistrarTarea(int numeroCaso, RegistrarTareaRequestDto request);

    Task<IEnumerable<DTOTarea>> BuscarTareas(int numeroCaso);

    Task<IEnumerable<DTOTipoTarea>> BuscarTiposTarea();
}

public class ExpertoTarea : IExpertoTarea
{
    private readonly IndireccionPersistencia _persistencia;

    public ExpertoTarea(IndireccionPersistencia persistencia)
    {
        _persistencia = persistencia;
    }

    public async Task RegistrarTarea(int numeroCaso, RegistrarTareaRequestDto request)
    {
        var especialistas = await _persistencia.Buscar("Especialista",
                                                        $"Legajo == {request.NroLegajoEspecialista} AND FechaHoraBaja == null");
        var especialista = especialistas.Cast<Especialista>().FirstOrDefault()
            ?? throw new BusinessException("No se ha podido encontrar el Especialista ingresado.");

        var caso = await ObtenerCaso(numeroCaso);

        var instanciaAsignada = caso.Instancias.FirstOrDefault(i =>
            i.EstadoActual?.Nombre == "Asignada" && i.EspecialistaId == especialista.Id)
            ?? throw new BusinessException("El especialista no tiene ninguna instancia asignada en este caso.");

        instanciaAsignada.Tareas.Add(new CasoInstanciaTarea
        {
            TipoTareaId = request.TipoTareaId,
            Observaciones = request.Observaciones ?? string.Empty,
            FechaHoraInicio = DateTime.UtcNow,
            FechaHoraFin = DateTime.UtcNow
        });

        await _persistencia.Guardar(caso);
    }

    public async Task<IEnumerable<DTOTarea>> BuscarTareas(int numeroCaso)
    {
        var caso = await ObtenerCaso(numeroCaso);

        return caso.Instancias
            .SelectMany(i => i.Tareas.Select(t => new DTOTarea(
                t.Id,
                i.OrdenCasoInstancia,
                t.TipoTareaId,
                t.Observaciones,
                t.FechaHoraInicio,
                t.FechaHoraFin)))
            .ToList();
    }

    public async Task<IEnumerable<DTOTipoTarea>> BuscarTiposTarea()
    {
        var resultado = await _persistencia.Buscar("TipoTarea", "FechaHoraBaja == null");

        return resultado.Cast<TipoTarea>().Select(t => new DTOTipoTarea(
            t.Id,
            t.Nombre,
            t.Descripcion));
    }

    private async Task<Caso> ObtenerCaso(int numeroCaso)
    {
        var resultado = await _persistencia.Buscar("Caso", $"NumeroCaso == {numeroCaso}");

        return resultado.Cast<Caso>().FirstOrDefault()
            ?? throw new BusinessException("No se ha podido encontrar el Caso ingresado.");
    }
}
