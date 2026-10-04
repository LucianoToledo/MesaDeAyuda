namespace MesaDeAyuda.Domain.Entities;

public class Caso
{
    public int Id { get; set; }
    public int NumeroCaso { get; set; }
    public DateTime FechaHoraIngreso { get; set; }
    public DateTime? FechaHoraFinCaso { get; set; }
    public DateTime? FechaHoraCaducidad { get; set; }
    public int NumeroIteracion { get; set; }
    public int NumeroCliente { get; set; }
    public string MailCliente { get; set; } = string.Empty;
    public string NumeroTelefonoCliente { get; set; } = string.Empty;
    public string Observaciones { get; set; } = string.Empty;

    // Relación con TipoCaso
    public int TipoCasoId { get; set; }

    public TipoCaso? TipoCaso { get; set; }

    // Relación con EstadoCaso (Estado actual del caso)
    public int EstadoId { get; set; }

    public EstadoCaso? EstadoActual { get; set; }

    // Relación 1..* con CasoInstancia
    public ICollection<CasoInstancia> Instancias { get; set; } = new List<CasoInstancia>();
}