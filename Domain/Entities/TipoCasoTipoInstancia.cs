namespace MesaDeAyuda.Domain.Entities;

public class TipoCasoTipoInstancia
{
    public int Id { get; set; }
    public int Orden { get; set; }
    public int MinutosMaximaResolucion { get; set; }
    public DateTime FechaAlta { get; set; }
    public DateTime? FechaBaja { get; set; }
    public DateTime? FechaHoraVerificacion { get; set; }

    public int TipoCasoId { get; set; }
    public TipoCaso? TipoCaso { get; set; }

    public int TipoInstanciaId { get; set; }
    public TipoInstancia? TipoInstancia { get; set; }

    public int TipoValidacionCierreId { get; set; }
    public TipoValidacionCierre? TipoValidacionCierre { get; set; }
}