using System.Linq.Dynamic.Core;
using System.Reflection;
using MesaDeAyuda.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace MesaDeAyuda.Persistencia;

// Patrón Indirección (ver documentación del proyecto, Sección 9): desacopla a los Expertos de las
// clases encargadas de la persistencia de entidades. La condición de búsqueda se recibe como texto,
// con la forma <atributo> <operador> <valor> {AND | OR <atributo> <operador> <valor>}, tal como la
// define el apunte de cátedra sobre Indirección de Persistencia. No se puede buscar por atributos de
// un objeto relacionado: primero hay que buscar ese objeto y recién después filtrar por él.
public class IndireccionPersistencia
{
    // DbContext solo expone Set<TEntity>() genérico (no hay una sobrecarga Set(Type)), y acá el
    // tipo de la clase a buscar recién se conoce en tiempo de ejecución (llega como string). Por
    // eso se resuelve el DbSet<TEntity> correspondiente invocando ese método genérico por reflexión.
    private static readonly MethodInfo _setMethod = typeof(DbContext).GetMethod(nameof(DbContext.Set), 1, Type.EmptyTypes)!;

    private readonly MesaAyudaDbContext _context;

    public IndireccionPersistencia(MesaAyudaDbContext context)
    {
        _context = context;
    }

    public async Task<List<object>> Buscar(string clase, string condicion)
    {
        var tipoEntidad = _context.Model.GetEntityTypes()
            .Select(e => e.ClrType)
            .FirstOrDefault(t => t.Name == clase)
            ?? throw new InvalidOperationException($"No existe la clase '{clase}'.");

        var consulta = (IQueryable)_setMethod.MakeGenericMethod(tipoEntidad).Invoke(_context, null)!;

        if (!string.IsNullOrWhiteSpace(condicion))
            consulta = consulta.Where(condicion);

        var resultado = await consulta.ToDynamicListAsync();

        return resultado.Cast<object>().ToList();
    }

    public async Task Guardar(object objeto)
    {
        _context.Update(objeto);
        await _context.SaveChangesAsync();
    }
}
