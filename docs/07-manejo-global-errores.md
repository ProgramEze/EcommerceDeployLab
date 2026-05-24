# Manejo global de errores y validaciones de API

## Objetivo

En este entregable se implementó un middleware global para manejar errores de forma centralizada.

El objetivo es evitar que la API devuelva errores genéricos o poco claros cuando ocurre una excepción controlada del dominio.

## Problema detectado

Antes de este entregable, si se enviaban datos inválidos al crear o actualizar productos, el dominio podía lanzar una `DomainException`.

Por ejemplo:

- Precio igual a cero.
- Stock negativo.
- Nombre vacío.
- Descripción vacía.

Sin un manejo adecuado, estas excepciones podían convertirse en respuestas HTTP `500 Internal Server Error`.

Esto no era correcto, porque esos errores no representan una falla inesperada del servidor, sino una validación de negocio.

## Solución implementada

Se creó el middleware:

```text
backend/src/Ecommerce.Api/Middlewares/ExceptionHandlingMiddleware.cs
```

Este middleware intercepta excepciones producidas durante la ejecución de la request.

## Manejo de DomainException

Cuando se captura una `DomainException`, la API devuelve:

```http
400 Bad Request
```

Con una respuesta JSON:

```json
{
	"message": "El precio del producto debe ser mayor que cero."
}
```

Esto permite que el cliente entienda que el problema está en los datos enviados.

## Manejo de errores inesperados

Cuando ocurre una excepción no controlada, la API devuelve:

```http
500 Internal Server Error
```

Con una respuesta genérica:

```json
{
	"message": "Ocurrió un error inesperado en el servidor."
}
```

El detalle técnico del error se registra mediante logs, pero no se expone al cliente.

Esto es importante porque una API no debería mostrar detalles internos como stack traces, nombres de clases, rutas internas o información sensible.

## Registro del middleware

El middleware se registró en `Program.cs` con:

```csharp
app.UseMiddleware<ExceptionHandlingMiddleware>();
```

El middleware debe ejecutarse antes de los endpoints para poder capturar errores generados por controladores, servicios o dominio.

## Por qué usar un middleware global

Un middleware global permite centralizar el manejo de errores en un solo lugar.

Esto evita repetir bloques `try/catch` en cada controller.

También permite mantener respuestas HTTP consistentes en toda la API.

## Flujo general

Cuando ocurre un error, el flujo es:

```text
Request HTTP
    ↓
ExceptionHandlingMiddleware
    ↓
Controller
    ↓
Application Service
    ↓
Domain
    ↓
Si ocurre DomainException
    ↓
Middleware devuelve 400 Bad Request
```

## Pruebas realizadas

Se probaron requests inválidas desde Swagger.

Ejemplo con precio inválido:

```json
{
	"name": "Producto inválido",
	"description": "Producto con precio inválido.",
	"price": 0,
	"stock": 5,
	"imageUrl": null
}
```

Respuesta esperada:

```http
400 Bad Request
```

```json
{
	"message": "El precio del producto debe ser mayor que cero."
}
```

También se probó que un producto válido siga funcionando correctamente:

```json
{
	"name": "Mouse Logitech",
	"description": "Mouse inalámbrico para oficina.",
	"price": 50,
	"stock": 10,
	"imageUrl": "https://example.com/mouse.jpg"
}
```

Respuesta esperada:

```http
201 Created
```

## Resultado

Al finalizar este entregable, la API responde de forma más clara y profesional ante errores de negocio.

El sistema ahora diferencia entre:

- Errores esperados de negocio.
- Errores inesperados del servidor.

Esto mejora la calidad de la API y prepara el proyecto para crecer con más módulos.
