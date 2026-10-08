using MesaDeAyuda.Domain.Entities;
using MesaDeAyuda.Domain.Exceptions;
using MesaDeAyuda.DTOs;
using MesaDeAyuda.Persistencia;

namespace MesaDeAyuda.Expertos;

// Utilidad de desarrollo/testing, NO es el CU que se está desarrollando en este TP
// (ver documentación del proyecto, "Endpoints de desarrollo/utilidad"). Provee la bandeja
// de casos asignados a un especialista para facilitar la demostración del flujo completo.
public interface IExpertoBandeja
{
    Task<IEnumerable<DTOCasoBandeja>> ObtenerBandeja(int nroLegajoEspecialista);
}

public class ExpertoBandeja : IExpertoBandeja
{
    private readonly IndireccionPersistencia _persistencia;

    public ExpertoBandeja(IndireccionPersistencia persistencia)
    {
        _persistencia = persistencia;
    }

    public async Task<IEnumerable<DTOCasoBandeja>> ObtenerBandeja(int nroLegajoEspecialista)
    {
        if (nroLegajoEspecialista <= 0)
            throw new BusinessException("Los datos ingresados son incorrectos. Intente nuevamente.");

        var especialistas = await _persistencia.Buscar("Especialista",
                                                        $"Legajo == {nroLegajoEspecialista} AND FechaHoraBaja == null");
        var especialista = especialistas.Cast<Especialista>().FirstOrDefault()
            ?? throw new BusinessException("No se ha podido encontrar el Especialista ingresado. Intente nuevamente.");

        var estadosTomado = await _persistencia.Buscar("EstadoCaso",
                                                        "Nombre == \"Tomado\" AND FechaHoraBaja == null");
        var estadoTomado = estadosTomado.Cast<EstadoCaso>().FirstOrDefault()
            ?? throw new BusinessException("No se encontró el estado 'Tomado'.");

        var casos = await _persistencia.Buscar("Caso", $"EstadoId == {estadoTomado.Id}");

        return casos.Cast<Caso>()
            .Where(c => c.Instancias.Any(i =>
                i.EspecialistaId == especialista.Id &&
                i.EstadoActual?.Nombre == "Asignada"))
            .Select(c =>
            {
                var instancia = c.Instancias.First(i =>
                    i.EspecialistaId == especialista.Id &&
                    i.EstadoActual?.Nombre == "Asignada");

                return new DTOCasoBandeja(
                    c.NumeroCaso,
                    c.FechaHoraIngreso,
                    c.EstadoActual!.Nombre,
                    instancia.TipoInstancia!.Nombre,
                    instancia.EstadoActual!.Nombre
                );
            })
            .ToList();
    }
}
