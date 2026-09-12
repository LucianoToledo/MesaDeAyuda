namespace MesaDeAyuda.Domain.Entities;

public class TipoInstancia
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public DateTime? FechaHoraBaja { get; set; }

    public int SectorId { get; set; }
    public Sector? Sector { get; set; }

    public ICollection<TipoCasoTipoInstancia> TiposCasoTipoInstancia { get; set; } = new List<TipoCasoTipoInstancia>();
}
