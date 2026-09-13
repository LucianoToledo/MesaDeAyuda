using MesaDeAyuda.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MesaDeAyuda.Infrastructure.Data;

public class MesaAyudaDbContext : DbContext
{
    public MesaAyudaDbContext(DbContextOptions<MesaAyudaDbContext> options) : base(options)
    {
    }

    public DbSet<Caso> Caso { get; set; }
    public DbSet<CasoInstancia> CasoInstancia { get; set; }
    public DbSet<CasoInstanciaTarea> CasoInstanciaTarea { get; set; }
    public DbSet<Especialista> Especialista { get; set; }
    public DbSet<Sector> Sector { get; set; }
    public DbSet<TipoCaso> TipoCaso { get; set; }
    public DbSet<TipoInstancia> TipoInstancia { get; set; }
    public DbSet<TipoTarea> TipoTarea { get; set; }
    public DbSet<TipoCasoTipoInstancia> TipoCasoTipoInstancia { get; set; }
    public DbSet<TipoCasoIteracion> TipoCasoIteracion { get; set; }
    public DbSet<EstadoCaso> EstadoCaso { get; set; }
    public DbSet<EstadoCasoInstancia> EstadoCasoInstancia { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Sin esto, EF Core no relaciona "EstadoId" con la navegación "EstadoActual" (el nombre
        // no matchea la convención) y crea una shadow property "EstadoActualId" aparte, que el
        // seed nunca completa: Include(EstadoActual) siempre devolvía null aunque EstadoId estuviera bien.
        modelBuilder.Entity<Caso>()
            .HasOne(c => c.EstadoActual)
            .WithMany()
            .HasForeignKey(c => c.EstadoId);

        modelBuilder.Entity<CasoInstancia>()
            .HasOne(ci => ci.EstadoActual)
            .WithMany()
            .HasForeignKey(ci => ci.EstadoId);

        SeedData(modelBuilder);
    }

    private static void SeedData(ModelBuilder modelBuilder)
    {
        var fecha = new DateTime(2026, 9, 10, 12, 0, 0, DateTimeKind.Utc);

        modelBuilder.Entity<EstadoCaso>().HasData(
            new EstadoCaso { Id = 1, Nombre = "Tomado" },
            new EstadoCaso { Id = 2, Nombre = "Disponible" },
            new EstadoCaso { Id = 3, Nombre = "Cerrado" },
            new EstadoCaso { Id = 4, Nombre = "Terminado Sin Éxito en la Iteración" },
            new EstadoCaso { Id = 5, Nombre = "Terminado Sin Solución" }
        );

        modelBuilder.Entity<EstadoCasoInstancia>().HasData(
            new EstadoCasoInstancia { Id = 1, Nombre = "Asignada" },
            new EstadoCasoInstancia { Id = 2, Nombre = "Resuelto" },
            new EstadoCasoInstancia { Id = 3, Nombre = "Cancelada" },
            new EstadoCasoInstancia { Id = 4, Nombre = "Sin Resolver" },
            new EstadoCasoInstancia { Id = 5, Nombre = "A Asignar" },
            new EstadoCasoInstancia { Id = 6, Nombre = "Sin Asignar" },
            new EstadoCasoInstancia { Id = 7, Nombre = "Caducada" }
        );

        modelBuilder.Entity<Sector>().HasData(
            new Sector { Id = 1, Nombre = "Atención Telefónica", Descripcion = "Atención primaria vía telefónica." },
            new Sector { Id = 2, Nombre = "Soporte Técnico", Descripcion = "Soporte técnico de segundo nivel." },
            new Sector { Id = 3, Nombre = "Redes", Descripcion = "Especialistas en infraestructura de red." }
        );

        modelBuilder.Entity<Especialista>().HasData(
            new Especialista { Id = 1, Legajo = 1001, Cuit = 201111116, NombreApellido = "Juan Pérez", SectorId = 1 },
            new Especialista { Id = 2, Legajo = 1002, Cuit = 272222223, NombreApellido = "Ana Gómez", SectorId = 2 },
            new Especialista { Id = 3, Legajo = 1003, Cuit = 203333334, NombreApellido = "Carlos López", SectorId = 3 }
        );

        modelBuilder.Entity<TipoTarea>().HasData(
            new TipoTarea { Id = 1, Nombre = "Llamada Saliente", Descripcion = "Contacto telefónico realizado hacia el cliente." },
            new TipoTarea { Id = 2, Nombre = "Diagnóstico Técnico", Descripcion = "Revisión técnica del problema reportado." },
            new TipoTarea { Id = 3, Nombre = "Derivación Interna", Descripcion = "Derivación del caso a otro sector o especialista." }
        );

        modelBuilder.Entity<TipoInstancia>().HasData(
            new TipoInstancia { Id = 1, Nombre = "Atención Telefónica", SectorId = 1 },
            new TipoInstancia { Id = 2, Nombre = "Soporte Técnico", SectorId = 2 },
            new TipoInstancia { Id = 3, Nombre = "Redes", SectorId = 3 }
        );

        modelBuilder.Entity<TipoCaso>().HasData(
            new TipoCaso { Id = 1, Nombre = "Falla de Conexión", NumeroMaximaIteracion = 3 }
        );

        // Coeficiente de reducción de tiempos por iteración (Regla de Negocio N°5, presión ERE).
        // La iteración 1 es la original, sin reducción; a partir de la 2 se aplica el coeficiente.
        modelBuilder.Entity<TipoCasoIteracion>().HasData(
            new TipoCasoIteracion { Id = 1, NumeroDeIteracion = 2, CoeficienteReduccionTipo = 75, TipoCasoId = 1 },
            new TipoCasoIteracion { Id = 2, NumeroDeIteracion = 3, CoeficienteReduccionTipo = 50, TipoCasoId = 1 }
        );

        // TipoCasoTipoInstancia reemplaza a ConfiguracionInstancia: agrega vigencia (FechaAlta/FechaBaja) sobre la
        // misma configuración (orden + tiempo máximo, ahora en minutos) que antes vivía en ConfiguracionInstancia.
        modelBuilder.Entity<TipoCasoTipoInstancia>().HasData(
            new TipoCasoTipoInstancia { Id = 1, Orden = 1, MinutosMaximaResolucion = 1440, FechaAlta = fecha.AddYears(-1), TipoCasoId = 1, TipoInstanciaId = 1, TipoValidacionCierre = TipoValidacionCierre.Simple },
            new TipoCasoTipoInstancia { Id = 2, Orden = 2, MinutosMaximaResolucion = 2880, FechaAlta = fecha.AddYears(-1), TipoCasoId = 1, TipoInstanciaId = 2, TipoValidacionCierre = TipoValidacionCierre.PorTareaRegistrada },
            new TipoCasoTipoInstancia { Id = 3, Orden = 3, MinutosMaximaResolucion = 4320, FechaAlta = fecha.AddYears(-1), TipoCasoId = 1, TipoInstanciaId = 3, TipoValidacionCierre = TipoValidacionCierre.PorObservaciones }
        );

        // Caso 1001: instancia 1 "Asignada" a Juan Pérez (legajo 1001) — camino feliz (Resuelto / NoResuelto con siguiente instancia disponible).
        // Caso 1002: instancias 1 y 2 ya "Sin Resolver", instancia 3 (última) "Asignada" a Carlos López (legajo 1003) — para probar C.A. N°6.
        // Caso 1003: en estado "Disponible" (no "Tomado") — para probar C.A. N°3 (caso no encontrado).
        modelBuilder.Entity<Caso>().HasData(
            new Caso { Id = 1, NumeroCaso = 1001, FechaHoraIngreso = fecha, FechaHoraCaducidad = fecha.AddDays(6), NumeroIteracion = 1, NumeroCliente = 5001, Observaciones = "Cliente reporta caída intermitente de conexión.", TipoCasoId = 1, EstadoId = 1 },
            new Caso { Id = 2, NumeroCaso = 1002, FechaHoraIngreso = fecha, FechaHoraCaducidad = fecha.AddDays(6), NumeroIteracion = 1, NumeroCliente = 5002, Observaciones = "Reclamo por corte total del servicio.", TipoCasoId = 1, EstadoId = 1 },
            new Caso { Id = 3, NumeroCaso = 1003, FechaHoraIngreso = fecha, FechaHoraCaducidad = fecha.AddDays(6), NumeroIteracion = 1, NumeroCliente = 5003, Observaciones = "Consulta por lentitud de conexión.", TipoCasoId = 1, EstadoId = 2 }
        );

        modelBuilder.Entity<CasoInstancia>().HasData(
            // Caso 1001
            new CasoInstancia { Id = 1, OrdenCasoInstancia = 1, FechaHoraInicioReal = fecha, FechaHoraInicioPlanificada = fecha, FechaHoraFinPlanificada = fecha.AddDays(1), CasoId = 1, EspecialistaId = 1, TipoInstanciaId = 1, EstadoId = 1 },
            new CasoInstancia { Id = 2, OrdenCasoInstancia = 2, FechaHoraFinPlanificada = fecha.AddDays(2), CasoId = 1, TipoInstanciaId = 2, EstadoId = 6 },
            new CasoInstancia { Id = 3, OrdenCasoInstancia = 3, FechaHoraFinPlanificada = fecha.AddDays(3), CasoId = 1, TipoInstanciaId = 3, EstadoId = 6 },

            // Caso 1002
            new CasoInstancia { Id = 4, OrdenCasoInstancia = 1, FechaHoraInicioReal = fecha, FechaHoraFinReal = fecha.AddDays(1), FechaHoraInicioPlanificada = fecha, FechaHoraFinPlanificada = fecha.AddDays(1), Observaciones = "No se logró restablecer el servicio en el primer contacto.", CasoId = 2, EspecialistaId = 1, TipoInstanciaId = 1, EstadoId = 4 },
            new CasoInstancia { Id = 5, OrdenCasoInstancia = 2, FechaHoraInicioReal = fecha.AddDays(1), FechaHoraFinReal = fecha.AddDays(3), FechaHoraInicioPlanificada = fecha.AddDays(1), FechaHoraFinPlanificada = fecha.AddDays(3), Observaciones = "Se descarta falla en el domicilio del cliente.", CasoId = 2, EspecialistaId = 2, TipoInstanciaId = 2, EstadoId = 4 },
            new CasoInstancia { Id = 6, OrdenCasoInstancia = 3, FechaHoraInicioReal = fecha.AddDays(3), FechaHoraInicioPlanificada = fecha.AddDays(3), FechaHoraFinPlanificada = fecha.AddDays(6), CasoId = 2, EspecialistaId = 3, TipoInstanciaId = 3, EstadoId = 1 },

            // Caso 1003
            new CasoInstancia { Id = 7, OrdenCasoInstancia = 1, FechaHoraFinPlanificada = fecha.AddDays(1), CasoId = 3, TipoInstanciaId = 1, EstadoId = 5 },
            new CasoInstancia { Id = 8, OrdenCasoInstancia = 2, FechaHoraFinPlanificada = fecha.AddDays(2), CasoId = 3, TipoInstanciaId = 2, EstadoId = 6 },
            new CasoInstancia { Id = 9, OrdenCasoInstancia = 3, FechaHoraFinPlanificada = fecha.AddDays(3), CasoId = 3, TipoInstanciaId = 3, EstadoId = 6 }
        );
    }
}