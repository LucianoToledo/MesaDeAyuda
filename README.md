# MesaDeAyuda

Transferencia de Conocimiento de las Prácticas Profesionales Supervisadas (PPS), 2do semestre, materia Diseño de Sistemas de Información (UTN, Facultad Regional Mendoza).

Parte de un negocio de Mesa de Ayuda ya relevado y especificado por la cátedra: una empresa de servicios necesita automatizar el seguimiento de los casos que reportan sus clientes, bajo presión de un ente regulador (ERE) que aplica multas por demoras en la atención. El caso de uso implementado, **Asentar Resultado**, es el mecanismo central de ese seguimiento: permite a un especialista registrar si logró resolver la instancia de un caso a su cargo (resuelto o no resuelto), lo cual dispara el resto del comportamiento del sistema (cierre del caso, avance a la siguiente instancia, o inicio de una nueva iteración).

El objetivo del proyecto no es relevar el negocio desde cero, sino tomar ese caso de uso ya especificado y extenderlo con dos requerimientos nuevos, elegidos porque cada uno ejemplifica el tipo de variación que resuelve un patrón GoF de comportamiento: un criterio de validación configurable antes de aceptar el resultado de una instancia (patrón **Strategy**), y la notificación al cliente por un canal que puede cambiar con el tiempo (patrón **Adapter**).

## Stack

- .NET 8 / ASP.NET Core Web API
- Entity Framework Core 8 + Npgsql (PostgreSQL)
- Swashbuckle (Swagger / OpenAPI)

## Arquitectura

Capas simplificadas en un único proyecto, pensadas para desacoplar responsabilidades y dejar lugar a los patrones de diseño aplicados:

| Carpeta | Responsabilidad |
|---|---|
| `Controllers/` | Endpoints REST. Reciben DTOs, invocan al Experto correspondiente y devuelven DTOs. Sin lógica de negocio. |
| `DTOs/` | Objetos de transferencia hacia/desde la API. Las entidades de dominio nunca se exponen directamente. |
| `Expertos/` | Orquestación y lógica de negocio del caso de uso. |
| `Strategies/` | Variaciones algorítmicas seleccionables en tiempo de ejecución (ver Strategy más abajo). |
| `Adapters/` | Integración con mecanismos externos de notificación (ver Adapter más abajo). |
| `Domain/Entities/` | Modelo de dominio. |
| `Domain/Exceptions/` | Excepciones propias del dominio (`BusinessException`). |
| `Repositories/` | Acceso a datos a través de interfaces. |
| `Infrastructure/Data/` | `DbContext`, mapeos de EF Core y seed de datos. |
| `Filters/` | Filtros globales de MVC (`BusinessExceptionFilter`, traduce `BusinessException` a `400`). |

## Patrones de diseño aplicados

- **Strategy** (validación de condiciones de cierre): antes de aceptar el resultado de una instancia, el sistema valida que el especialista haya dejado una constancia mínima del trabajo realizado. Qué se exige (nada, observaciones completas, o al menos una tarea registrada) es un dato configurable por tipo de instancia, y cada variante es una estrategia distinta seleccionada por una factoría.
- **Adapter** (notificación al cliente): al cerrarse un caso como resuelto, el sistema notifica al cliente por un canal (email, SMS). El Adapter permite incorporar nuevos canales o proveedores sin modificar el resto del sistema.

## Cómo levantarlo

**Requisitos:** .NET 8 SDK, una instancia de PostgreSQL accesible.

1. Configurar la cadena de conexión en `appsettings.json` (o `appsettings.Development.json`), clave `ConnectionStrings:DefaultConnection`.
2. Aplicar las migraciones:
   ```
   dotnet ef database update
   ```
3. Levantar la API:
   ```
   dotnet run
   ```
4. Swagger UI queda disponible en `/swagger`.

Para volver la base al estado inicial del seed en cualquier momento (solo en entorno `Development`):
```
POST api/v1/seed/reset
```

## Endpoints del caso de uso

Los tres pasos de Asentar Resultado, pensados para llamarse en secuencia desde la interfaz:

**1. `GET api/v1/especialistas/{nroLegajoEspecialista}`**

Es el primer paso del caso de uso: antes de que el especialista pueda operar sobre un caso, el sistema confirma su identidad, que el legajo exista y esté activo. Corresponde al momento en que el especialista se identifica para empezar a trabajar.

- Respuesta: `DTOEspecialista` (legajo, CUIT, nombre y apellido).
- Tablas: lee `Especialista`. No escribe nada.
- Errores: `400` si el legajo no existe.

**2. `GET api/v1/casos/{numeroCaso}?nroLegajoEspecialista={nroLegajoEspecialista}`**

Ya identificado, el especialista selecciona el caso puntual que va a resolver, de los que tiene en su bandeja. Este paso confirma que el caso está efectivamente en curso y que la instancia pendiente le pertenece a él y no a otro especialista, y le muestra los datos del caso antes de que registre el resultado de su trabajo.

- Respuesta: `DTOCaso` (número, cliente, fecha de ingreso, estado, observaciones del caso).
- Tablas: lee `Especialista`, `Caso`, `CasoInstancia` (para ubicar la instancia asignada) y los catálogos `EstadoCaso`/`EstadoCasoInstancia`. No escribe nada.
- Errores: `400` si el caso no existe, no está `Tomado`, no tiene una instancia `Asignada`, o esa instancia pertenece a otro especialista.

**3. `POST api/v1/casos/asentar-resultado`**

Es el corazón del caso de uso: el especialista, después de haber trabajado la instancia a su cargo, deja constancia de si logró resolverla o no. Ese registro dispara el resto del comportamiento del sistema: si fue exitoso, el caso se cierra, se cancelan las instancias que quedaban pendientes y se notifica al cliente; si no, el caso pasa al sector responsable de la siguiente instancia o, si era la última, la iteración actual termina sin éxito. También es el punto donde se exige la documentación mínima que corresponda al tipo de instancia antes de aceptar cualquiera de los dos resultados.

- Body: `{ nroLegajoEspecialista, numeroCaso, respuesta, observaciones }` (`respuesta`: `true` = Resuelto, `false` = Sin Resolver).
- Respuesta: `204 No Content`.
- Tablas: lee `Especialista`, `Caso` + `CasoInstancia` + `CasoInstanciaTarea` (para la validación de cierre), `TipoCasoTipoInstancia` (para saber qué validación exige esa instancia) y los catálogos de estado. Escribe `CasoInstancia` (observaciones, fecha de fin real, estado) de la instancia actual y, según el resultado, de la instancia siguiente o de las que se cancelan; y `Caso` (estado y, si corresponde, fecha de fin). No crea filas nuevas, solo actualiza las existentes.

## Endpoints auxiliares

Agregados para poder probar el flujo completo sin insertar o actualizar filas a mano en Postgres. No especifican formalmente ningún caso de uso propio y no forman parte del diagrama de clases de Asentar Resultado.

- **`POST api/v1/seed/reset`** — Borra la base entera y la recrea aplicando la migración (incluye el seed de datos de prueba). Solo responde en `Development`. Tablas: afecta toda la base.
- **`POST api/v1/casos/tomar`** — Asigna al especialista la instancia `A Asignar` del caso que corresponda a su sector. Body: `{ nroLegajoEspecialista, numeroCaso }`. Tablas: lee `Especialista`, `Caso` + `CasoInstancia` + `TipoInstancia` (sector de cada instancia); escribe `CasoInstancia` (especialista, estado `Asignada`, fecha de inicio real) y `Caso` (estado `Tomado`).
- **`POST api/v1/casos/{numeroCaso}/tareas`** — Registra una tarea contra la instancia asignada al especialista en ese caso. Body: `{ nroLegajoEspecialista, tipoTareaId, observaciones }`. Tablas: lee `Especialista`, `Caso` + `CasoInstancia`; escribe una fila nueva en `CasoInstanciaTarea`.
- **`GET api/v1/casos/{numeroCaso}/tareas`** — Lista las tareas ya registradas en las instancias de un caso. Tablas: lee `Caso` + `CasoInstancia` + `CasoInstanciaTarea`.
- **`GET api/v1/tipos-tarea`** — Lista el catálogo de tipos de tarea, para saber qué `tipoTareaId` son válidos al registrar una tarea. Tablas: lee `TipoTarea`.

## Ejemplo de flujo de punta a punta

Con los datos del seed (caso `1001`, especialista `1001` con la instancia 1 asignada):

1. `GET api/v1/especialistas/1001` — confirma el especialista.
2. `GET api/v1/casos/1001?nroLegajoEspecialista=1001` — confirma el caso y la pertenencia.
3. `POST api/v1/casos/asentar-resultado` con `{ "nroLegajoEspecialista": 1001, "numeroCaso": 1001, "respuesta": false }` — la instancia 1 pasa a "Sin Resolver", la instancia 2 pasa a "A Asignar", el caso vuelve a "Disponible".
4. `POST api/v1/casos/tomar` con `{ "nroLegajoEspecialista": 1002, "numeroCaso": 1001 }` — el especialista del sector de la instancia 2 la toma.
5. `POST api/v1/casos/1001/tareas` con `{ "nroLegajoEspecialista": 1002, "tipoTareaId": 2, "observaciones": "..." }` — la instancia 2 exige al menos una tarea registrada antes de poder cerrarla.
6. `POST api/v1/casos/asentar-resultado` con `{ "nroLegajoEspecialista": 1002, "numeroCaso": 1001, "respuesta": true }` — el caso se cierra, la instancia 3 (nunca tomada) se cancela, y se dispara la notificación al cliente.

## Estado y alcance

Implementado: el Camino Básico completo del caso de uso, sus caminos alternos (incluyendo el avance a la siguiente instancia cuando una falla y no es la última), las estrategias de validación de cierre y el adapter de notificación.

Pendiente: el caso de uso `IterarCaso` (regenerar la secuencia de instancias con tiempos reducidos cuando falla la última instancia de una iteración) todavía no está diseñado. Hoy, cuando eso ocurre, el caso queda en estado `Terminado Sin Éxito en la Iteración` sin generar automáticamente una nueva iteración.
