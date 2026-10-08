namespace MesaDeAyuda.DTOs;

public record DTOCasoBandeja(
    int NroCaso,
    DateTime FechaHoraIngresoCaso,
    string NombreEstadoCaso,
    string NombreTipoInstanciaActual,
    string NombreEstadoInstanciaActual
);
