using MesaDeAyuda.Expertos;
using MesaDeAyuda.Filters;
using MesaDeAyuda.Infrastructure.Data;
using MesaDeAyuda.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// --- CONFIGURACIÓN DE SERVICIOS (DI) ---

// Controllers y Swagger
builder.Services.AddControllers(options => options.Filters.Add<BusinessExceptionFilter>());
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFile));
});

// Rutas en minúscula (ej. /api/v1/casos en vez de /api/v1/Casos)
builder.Services.AddRouting(options => options.LowercaseUrls = true);

// Base de Datos (PostgreSQL)
builder.Services.AddDbContext<MesaAyudaDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Repositorios
builder.Services.AddScoped<ICasoRepository, CasoRepository>();
builder.Services.AddScoped<IEspecialistaRepository, EspecialistaRepository>();
builder.Services.AddScoped<IEstadoCasoRepository, EstadoCasoRepository>();
builder.Services.AddScoped<IEstadoCasoInstanciaRepository, EstadoCasoInstanciaRepository>();
builder.Services.AddScoped<ITipoCasoTipoInstanciaRepository, TipoCasoTipoInstanciaRepository>();
builder.Services.AddScoped<ITipoTareaRepository, TipoTareaRepository>();

// Experto (Capa de lógica de negocio)
builder.Services.AddScoped<IExpertoAsentarResultado, ExpertoAsentarResultado>();

// Expertos de utilidad de testing (no son parte del CU principal, ver documentación del proyecto, Sección 3)
builder.Services.AddScoped<IExpertoTomarCaso, ExpertoTomarCaso>();
builder.Services.AddScoped<IExpertoTarea, ExpertoTarea>();

var app = builder.Build();

// --- PIPELINE DE SOLICITUDES HTTP ---

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
