using MesaDeAyuda.Adapters;
using MesaDeAyuda.Domain.Entities;
using MesaDeAyuda.Domain.Exceptions;
using MesaDeAyuda.DTOs;
using MesaDeAyuda.Repositories;
using MesaDeAyuda.Strategies;

namespace MesaDeAyuda.Expertos;

public interface IExpertoAsentarResultado
{
    Task<DTOEspecialista> BuscarEspecialista(int nroLegajoEspecialista);

    Task<DTOCaso> BuscarCaso(int nroLegajoEspecialista, int numeroCaso);

    Task IngresarRespuesta(AsentarResultadoRequestDto request);
}

public class ExpertoAsentarResultado : IExpertoAsentarResultado
{
    private readonly ICasoRepository _casoRepository;
    private readonly IEspecialistaRepository _especialistaRepository;
    private readonly IEstadoCasoRepository _estadoCasoRepository;
    private readonly IEstadoCasoInstanciaRepository _estadoCasoInstanciaRepository;
    private readonly ITipoCasoTipoInstanciaRepository _tipoCasoTipoInstanciaRepository;

    public ExpertoAsentarResultado(ICasoRepository casoRepository,
                                   IEspecialistaRepository especialistaRepository,
                                   IEstadoCasoRepository estadoCasoRepository,
                                   IEstadoCasoInstanciaRepository estadoCasoInstanciaRepository,
                                   ITipoCasoTipoInstanciaRepository tipoCasoTipoInstanciaRepository)
    {
        _casoRepository = casoRepository;
        _especialistaRepository = especialistaRepository;
        _estadoCasoRepository = estadoCasoRepository;
        _estadoCasoInstanciaRepository = estadoCasoInstanciaRepository;
        _tipoCasoTipoInstanciaRepository = tipoCasoTipoInstanciaRepository;
    }

    public async Task<DTOEspecialista> BuscarEspecialista(int nroLegajoEspecialista)
    {
        var especialista = await ValidarEspecialista(nroLegajoEspecialista);

        return new DTOEspecialista(especialista.Legajo,
                                   especialista.Cuit,
                                   especialista.NombreApellido);
    }

    // Desvío deliberado del DC (ver documentación del proyecto, Sección 3): acá sí recibe el especialista, porque
    // la API es stateless y el C.A. N°4 debe comprobarse en este paso, no solo en IngresarRespuesta.
    public async Task<DTOCaso> BuscarCaso(int nroLegajoEspecialista, int numeroCaso)
    {
        var especialista = await ValidarEspecialista(nroLegajoEspecialista);
        var (caso, instanciaAsignada) = await ObtenerCasoConInstanciaAsignada(numeroCaso);

        ValidarPerteneceAEspecialista(instanciaAsignada,
                                      especialista);

        return new DTOCaso(caso.NumeroCaso,
                           caso.NumeroCliente,
                           caso.FechaHoraIngreso,
                           caso.EstadoActual!.Nombre,
                           caso.Observaciones);
    }

    public async Task IngresarRespuesta(AsentarResultadoRequestDto request)
    {
        var especialista = await ValidarEspecialista(request.NroLegajoEspecialista);
        var (caso, instanciaActual) = await ObtenerCasoConInstanciaAsignada(request.NumeroCaso);

        ValidarPerteneceAEspecialista(instanciaActual, especialista);

        instanciaActual.Observaciones = request.Observaciones ?? string.Empty;

        await ValidarCierreInstancia(caso, instanciaActual);

        if (request.Respuesta)
            await RegistrarResolucionExitosa(caso, instanciaActual);
        else
            await RegistrarInstanciaSinResolver(caso, instanciaActual);

        await _casoRepository.UpdateAsync(caso);
    }

    private async Task<Especialista> ValidarEspecialista(int nroLegajoEspecialista)
    {
        if (nroLegajoEspecialista <= 0)
            throw new BusinessException("Los datos ingresados son incorrectos. Intente nuevamente.");

        return await _especialistaRepository.GetByLegajoAsync(nroLegajoEspecialista)
            ?? throw new BusinessException("No se ha podido encontrar el Especialista ingresado. Intente nuevamente.");
    }

    private async Task<(Caso Caso, CasoInstancia InstanciaAsignada)> ObtenerCasoConInstanciaAsignada(int numeroCaso)
    {
        if (numeroCaso <= 0)
            throw new BusinessException("Los datos ingresados son incorrectos. Intente nuevamente.");

        var caso = await _casoRepository.GetByNumeroAsync(numeroCaso);
        if (caso is null || caso.EstadoActual?.Nombre != "Tomado")
            throw new BusinessException("No se ha podido encontrar el Caso ingresado. Intente nuevamente.");

        var instanciaAsignada = caso.Instancias.FirstOrDefault(i => i.EstadoActual?.Nombre == "Asignada");
        if (instanciaAsignada is null)
            throw new BusinessException("No se ha podido encontrar el Caso ingresado. Intente nuevamente.");

        return (caso, instanciaAsignada);
    }

    // Camino Alterno N°4: no coincide Especialista.
    private static void ValidarPerteneceAEspecialista(CasoInstancia instanciaAsignada, Especialista especialista)
    {
        if (instanciaAsignada.EspecialistaId != especialista.Id)
            throw new BusinessException("El caso seleccionado se encuentra asignado a otro Especialista. Ingrese un nuevo número de caso.");
    }

    // Nuevo Camino Alterno: rechaza el asentamiento si no se cumple la documentación mínima exigida
    // para el tipo de instancia (aplica tanto a "Resuelto" como a "Sin Resolver").
    private async Task ValidarCierreInstancia(Caso caso, CasoInstancia instanciaActual)
    {
        var configuracion = await _tipoCasoTipoInstanciaRepository.ObtenerVigente(caso.TipoCasoId,
                                                                                  instanciaActual.OrdenCasoInstancia,
                                                                                  DateTime.UtcNow);

        var tipoValidacion = configuracion?.TipoValidacionCierre ?? TipoValidacionCierre.Simple;
        var estrategia = FactoriaEstrategiaValidacionCierre.Instancia.ObtenerEstrategia(tipoValidacion);

        estrategia.ValidarCierre(instanciaActual);
    }

    // Camino Básico, pasos 9.1 a 9.8, + paso 9.9 agregado (notificar al cliente)
    private async Task RegistrarResolucionExitosa(Caso caso, CasoInstancia instanciaActual)
    {
        var estadoResuelto = await _estadoCasoInstanciaRepository.GetByNombreAsync("Resuelto") ?? throw new BusinessException("No se encontró el estado 'Resuelto'.");

        instanciaActual.FechaHoraFinReal = DateTime.UtcNow;
        instanciaActual.EstadoId = estadoResuelto.Id;
        instanciaActual.EstadoActual = estadoResuelto;

        var estadoCancelada = await _estadoCasoInstanciaRepository.GetByNombreAsync("Cancelada") ?? throw new BusinessException("No se encontró el estado 'Cancelada'.");

        foreach (var instancia in caso.Instancias.Where(i => i.Id != instanciaActual.Id && i.EstadoActual?.Nombre == "Sin Asignar"))
        {
            instancia.EstadoId = estadoCancelada.Id;
            instancia.EstadoActual = estadoCancelada;
        }

        var estadoCerrado = await _estadoCasoRepository.GetByNombreAsync("Cerrado") ?? throw new BusinessException("No se encontró el estado 'Cerrado'.");

        caso.FechaHoraFinCaso = DateTime.UtcNow;
        caso.EstadoId = estadoCerrado.Id;
        caso.EstadoActual = estadoCerrado;

        var adaptadorNotificacion = FactoriaAdaptadorNotificacionCliente.Instancia.ObtenerAdaptador();

        adaptadorNotificacion.Notificar(new DTONotificacionCliente(caso.NumeroCliente.ToString(),
                                                                   $"Su caso N° {caso.NumeroCaso} ha sido resuelto."));
    }

    // Camino Alterno N°5 (y, si corresponde, N°6): la instancia actual queda "Sin Resolver"
    private async Task RegistrarInstanciaSinResolver(Caso caso, CasoInstancia instanciaActual)
    {
        var estadoSinResolver = await _estadoCasoInstanciaRepository.GetByNombreAsync("Sin Resolver") ?? throw new BusinessException("No se encontró el estado 'Sin Resolver'.");

        instanciaActual.FechaHoraFinReal = DateTime.UtcNow;
        instanciaActual.EstadoId = estadoSinResolver.Id;
        instanciaActual.EstadoActual = estadoSinResolver;

        var siguienteInstancia = caso.Instancias.FirstOrDefault(i => i.OrdenCasoInstancia == instanciaActual.OrdenCasoInstancia + 1);

        if (siguienteInstancia is not null)
        {
            var estadoAAsignar = await _estadoCasoInstanciaRepository.GetByNombreAsync("A Asignar") ?? throw new BusinessException("No se encontró el estado 'A Asignar'.");

            siguienteInstancia.EstadoId = estadoAAsignar.Id;
            siguienteInstancia.EstadoActual = estadoAAsignar;

            var estadoDisponible = await _estadoCasoRepository.GetByNombreAsync("Disponible") ?? throw new BusinessException("No se encontró el estado 'Disponible'.");
            caso.EstadoId = estadoDisponible.Id;
            caso.EstadoActual = estadoDisponible;

            return;
        }

        // Camino Alterno N°6: la instancia sin resolver es la última de la iteración.
        var estadoTerminadoSinExito = await _estadoCasoRepository.GetByNombreAsync("Terminado Sin Éxito en la Iteración") ?? throw new BusinessException("No se encontró el estado 'Terminado Sin Éxito en la Iteración'.");

        caso.FechaHoraFinCaso = DateTime.UtcNow;
        caso.EstadoId = estadoTerminadoSinExito.Id;
        caso.EstadoActual = estadoTerminadoSinExito;

        // Camino Alterno N°7 / Inclusión CU IterarCaso: ese CU todavía no está especificado.
        // TODO: invocar CU IterarCaso(caso.NumeroCaso) cuando esté diseñado.
    }
}