using MesaDeAyuda.Adapters;
using MesaDeAyuda.Domain.Entities;
using MesaDeAyuda.Domain.Exceptions;
using MesaDeAyuda.DTOs;
using MesaDeAyuda.Persistencia;
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
    private readonly IndireccionPersistencia _persistencia;

    public ExpertoAsentarResultado(IndireccionPersistencia persistencia)
    {
        _persistencia = persistencia;
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

        await _persistencia.Guardar(caso);
    }

    private async Task<Especialista> ValidarEspecialista(int nroLegajoEspecialista)
    {
        if (nroLegajoEspecialista <= 0)
            throw new BusinessException("Los datos ingresados son incorrectos. Intente nuevamente.");

        var resultado = await _persistencia.Buscar("Especialista",
                                                    $"Legajo == {nroLegajoEspecialista} AND FechaHoraBaja == null");

        return resultado.Cast<Especialista>().FirstOrDefault()
            ?? throw new BusinessException("No se ha podido encontrar el Especialista ingresado. Intente nuevamente.");
    }

    private async Task<(Caso Caso, CasoInstancia InstanciaAsignada)> ObtenerCasoConInstanciaAsignada(int numeroCaso)
    {
        if (numeroCaso <= 0)
            throw new BusinessException("Los datos ingresados son incorrectos. Intente nuevamente.");

        var resultado = await _persistencia.Buscar("Caso", $"NumeroCaso == {numeroCaso}");
        var caso = resultado.Cast<Caso>().FirstOrDefault();

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
        var resultado = await _persistencia.Buscar("TipoCasoTipoInstancia",
                                                    $"TipoCasoId == {caso.TipoCasoId} AND Orden == {instanciaActual.OrdenCasoInstancia}");

        var fechaActual = DateTime.UtcNow;
        var configuracion = resultado.Cast<TipoCasoTipoInstancia>()
            .FirstOrDefault(t => t.FechaAlta <= fechaActual && (t.FechaBaja == null || t.FechaBaja > fechaActual));

        var tipoValidacion = configuracion?.TipoValidacionCierre ?? TipoValidacionCierre.Simple;
        var estrategia = FactoriaEstrategiaValidacionCierre.Instancia.ObtenerEstrategia(tipoValidacion);

        estrategia.ValidarCierre(instanciaActual);
    }

    // Camino Básico, pasos 9.1 a 9.8, + paso 9.9 agregado (notificar al cliente)
    private async Task RegistrarResolucionExitosa(Caso caso, CasoInstancia instanciaActual)
    {
        var estadoResuelto = await BuscarEstadoCasoInstancia("Resuelto");

        instanciaActual.FechaHoraFinReal = DateTime.UtcNow;
        instanciaActual.EstadoId = estadoResuelto.Id;
        instanciaActual.EstadoActual = estadoResuelto;

        var estadoCancelada = await BuscarEstadoCasoInstancia("Cancelada");

        foreach (var instancia in caso.Instancias.Where(i => i.Id != instanciaActual.Id && i.EstadoActual?.Nombre == "Sin Asignar"))
        {
            instancia.EstadoId = estadoCancelada.Id;
            instancia.EstadoActual = estadoCancelada;
        }

        var estadoCerrado = await BuscarEstadoCaso("Cerrado");

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
        var estadoSinResolver = await BuscarEstadoCasoInstancia("Sin Resolver");

        instanciaActual.FechaHoraFinReal = DateTime.UtcNow;
        instanciaActual.EstadoId = estadoSinResolver.Id;
        instanciaActual.EstadoActual = estadoSinResolver;

        var siguienteInstancia = caso.Instancias.FirstOrDefault(i => i.OrdenCasoInstancia == instanciaActual.OrdenCasoInstancia + 1);

        if (siguienteInstancia is not null)
        {
            var estadoAAsignar = await BuscarEstadoCasoInstancia("A Asignar");

            siguienteInstancia.EstadoId = estadoAAsignar.Id;
            siguienteInstancia.EstadoActual = estadoAAsignar;

            var estadoDisponible = await BuscarEstadoCaso("Disponible");
            caso.EstadoId = estadoDisponible.Id;
            caso.EstadoActual = estadoDisponible;

            return;
        }

        // Camino Alterno N°6: la instancia sin resolver es la última de la iteración.
        var estadoTerminadoSinExito = await BuscarEstadoCaso("Terminado Sin Éxito en la Iteración");

        caso.FechaHoraFinCaso = DateTime.UtcNow;
        caso.EstadoId = estadoTerminadoSinExito.Id;
        caso.EstadoActual = estadoTerminadoSinExito;

        // Camino Alterno N°7 / Inclusión CU IterarCaso: ese CU todavía no está especificado.
        // TODO: invocar CU IterarCaso(caso.NumeroCaso) cuando esté diseñado.
    }

    private async Task<EstadoCaso> BuscarEstadoCaso(string nombre)
    {
        var resultado = await _persistencia.Buscar("EstadoCaso", $"Nombre == \"{nombre}\" AND FechaHoraBaja == null");

        return resultado.Cast<EstadoCaso>().FirstOrDefault()
            ?? throw new BusinessException($"No se encontró el estado '{nombre}'.");
    }

    private async Task<EstadoCasoInstancia> BuscarEstadoCasoInstancia(string nombre)
    {
        var resultado = await _persistencia.Buscar("EstadoCasoInstancia", $"Nombre == \"{nombre}\" AND FechaHoraBaja == null");

        return resultado.Cast<EstadoCasoInstancia>().FirstOrDefault()
            ?? throw new BusinessException($"No se encontró el estado '{nombre}'.");
    }
}
