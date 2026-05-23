# Health check y Swagger

## Objetivo

En este entregable se limpió la API inicial generada por ASP.NET Core y se agregaron endpoints técnicos básicos.

El objetivo fue dejar una API mínima pero útil para verificar que el sistema está funcionando.

## Endpoints creados

Se crearon los siguientes endpoints:

```http
GET /
GET /health
```

## Endpoint raíz

El endpoint raíz devuelve información básica de la API.

Ejemplo de respuesta:

```json
{
  "application": "EcommerceDeployLab API",
  "status": "Running",
  "environment": "Development"
}
```

Este endpoint permite verificar rápidamente que la API está levantada.

## Endpoint de health check

El endpoint `/health` permite verificar si la aplicación está viva.

Respuesta esperada:

```text
Healthy
```

Este endpoint será útil más adelante para:

- Monitoreo.
- Diagnóstico.
- Azure.
- CI/CD.
- Validaciones de despliegue.
- Pruebas rápidas de disponibilidad.

## Swagger

Swagger se configuró para poder visualizar y probar los endpoints de la API desde el navegador.

URL local esperada:

```text
http://localhost:5075/swagger
```

El puerto puede variar según la configuración local.

## Limpieza inicial

Se eliminó el endpoint de ejemplo `WeatherForecast`, porque no pertenece al dominio del sistema.

El proyecto quedó preparado para agregar endpoints reales del e-commerce.

## Problema encontrado con Swagger

Durante la configuración apareció un conflicto relacionado con paquetes OpenAPI y Swagger.

El error estaba relacionado con una incompatibilidad entre:

- `Microsoft.OpenApi`
- `Microsoft.AspNetCore.OpenApi`
- `Swashbuckle.AspNetCore`
- `.WithOpenApi()`

## Solución aplicada

Se decidió usar una configuración simple con `Swashbuckle.AspNetCore`.

También se evitó temporalmente el uso de `.WithOpenApi()` para evitar conflictos de versiones.

La configuración final permitió que:

- La API compile correctamente.
- Swagger cargue correctamente.
- Los endpoints `/` y `/health` funcionen.

## Configuración relevante

En `Program.cs` se registraron servicios como:

```csharp
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHealthChecks();
```

Y luego se mapearon los endpoints:

```csharp
app.MapHealthChecks("/health");

app.MapGet("/", () => Results.Ok(new
{
    application = "EcommerceDeployLab API",
    status = "Running",
    environment = app.Environment.EnvironmentName
}));
```

## Resultado

Al finalizar este entregable, la API quedó funcionando con:

- Endpoint raíz.
- Endpoint de salud.
- Swagger operativo.
- Proyecto limpio, sin endpoints de ejemplo innecesarios.