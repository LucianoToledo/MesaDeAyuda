using MesaDeAyuda.Domain.Exceptions;
using MesaDeAyuda.DTOs;
using MesaDeAyuda.Repositories;

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
    private readonly ICasoRepository _casoRepository;
    private readonly IEspecialistaRepository _especialistaRepository;
    private readonly IEstadoCasoRepository _estadoCasoRepository;
    private readonly IEstadoCasoInstanciaRepository _estadoCasoInstanciaRepository;

    public ExpertoTomarCaso(
        ICasoRepository casoRepository,
        IEspecialistaRepository especialistaRepository,
        IEstadoCasoRepository estadoCasoRepository,
        IEstadoCasoInstanciaRepository estadoCasoInstanciaRepository)
    {
        _casoRepository = casoRepository;
        _especialistaRepository = especialistaRepository;
        _estadoCasoRepository = estadoCasoRepository;
        _estadoCasoInstanciaRepository = estadoCasoInstanciaRepository;
    }

    public async Task TomarInstancia(TomarCasoRequestDto request)
    {
        var especialista = await _especialistaRepository.GetByLegajoAsync(request.NroLegajoEspecialista)
            ?? throw new BusinessException("No se ha podido encontrar el Especialista ingresado.");

        var caso = await _casoRepository.GetByNumeroAsync(request.NumeroCaso)
            ?? throw new BusinessException("No se ha podido encontrar el Caso ingresado.");

        var instanciaDisponible = caso.Instancias.FirstOrDefault(i =>
            i.EstadoActual?.Nombre == "A Asignar" && i.TipoInstancia!.SectorId == especialista.SectorId)
            ?? throw new BusinessException("No hay ninguna instancia disponible en su sector para este caso.");

        var estadoAsignada = await _estadoCasoInstanciaRepository.GetByNombreAsync("Asignada")
            ?? throw new BusinessException("No se encontró el estado 'Asignada'.");

        instanciaDisponible.EspecialistaId = especialista.Id;
        instanciaDisponible.EstadoId = estadoAsignada.Id;
        instanciaDisponible.EstadoActual = estadoAsignada;
        instanciaDisponible.FechaHoraInicioReal = DateTime.UtcNow;

        var estadoTomado = await _estadoCasoRepository.GetByNombreAsync("Tomado")
            ?? throw new BusinessException("No se encontró el estado 'Tomado'.");

        caso.EstadoId = estadoTomado.Id;
        caso.EstadoActual = estadoTomado;

        await _casoRepository.UpdateAsync(caso);
    }
}
