namespace MesaDeAyuda.DTOs;

public record DTOTarea(
    int Id,
    int OrdenCasoInstancia,
    int TipoTareaId,
    string Observaciones,
    DateTime? FechaHoraInicio,
    DateTime? FechaHoraFin
);