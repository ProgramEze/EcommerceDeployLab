# Carrito de compras - API

## Objetivo

En este entregable se expusieron endpoints HTTP para operar con el carrito de compras.

El objetivo fue conectar la capa `Ecommerce.Api` con los casos de uso definidos en `Ecommerce.Application` y con la persistencia real implementada en `Ecommerce.Infrastructure`.

Con esto, el sistema permite crear carritos reales, consultar carritos, agregar productos, modificar cantidades, quitar productos, vaciar el carrito y eliminar el carrito completo.

## Controlador creado

Se creó el controlador:

```text
backend/src/Ecommerce.Api/Controllers/CartsController.cs
```

Este controlador usa la interfaz:

```text
ICartService
```

para acceder a los casos de uso del carrito.

## Endpoints creados

Se crearon los siguientes endpoints:

```http
POST   /api/carts
GET    /api/carts/{id}
POST   /api/carts/{cartId}/items
PUT    /api/carts/{cartId}/items/{productId}
DELETE /api/carts/{cartId}/items/{productId}
DELETE /api/carts/{cartId}/items
DELETE /api/carts/{id}
```

## POST /api/carts

Crea un carrito vacío.

Ejemplo:

```http
POST /api/carts
```

No requiere body.

Respuesta esperada:

```http
201 Created
```

Ejemplo de respuesta:

```json
{
	"id": "772139d0-b7a6-409f-bc00-0d9476a9cb4e",
	"items": [],
	"total": 0,
	"totalItems": 0,
	"isEmpty": true,
	"createdAt": "2026-05-24T00:32:47.662113Z",
	"updatedAt": null
}
```

## GET /api/carts/{id}

Devuelve un carrito por Id.

Ejemplo:

```http
GET /api/carts/772139d0-b7a6-409f-bc00-0d9476a9cb4e
```

Si el carrito existe, devuelve:

```http
200 OK
```

Si el carrito no existe, devuelve:

```http
404 Not Found
```

Con una respuesta como:

```json
{
	"message": "Carrito no encontrado."
}
```

## POST /api/carts/{cartId}/items

Agrega un producto al carrito.

Ejemplo:

```http
POST /api/carts/772139d0-b7a6-409f-bc00-0d9476a9cb4e/items
```

Body esperado:

```json
{
	"productId": "f379aff5-d017-41ef-b944-603d3aed2c78",
	"productName": "Mouse Logitech",
	"unitPrice": 50,
	"quantity": 5
}
```

Si el producto no existe en el carrito, se agrega como nuevo item.

Si el producto ya existe en el carrito, se incrementa su cantidad.

Ejemplo de respuesta:

```json
{
	"id": "e4393848-a7af-4f8a-a8a5-0ac3c38dca05",
	"items": [
		{
			"id": "df947b46-421f-4c9d-8f24-4ddfa908b124",
			"productId": "f379aff5-d017-41ef-b944-603d3aed2c78",
			"productName": "Mouse Logitech",
			"unitPrice": 50,
			"quantity": 5,
			"subtotal": 250
		}
	],
	"total": 250,
	"totalItems": 5,
	"isEmpty": false,
	"createdAt": "2026-05-24T01:12:24.560344Z",
	"updatedAt": "2026-05-24T01:13:09.9113769Z"
}
```

## PUT /api/carts/{cartId}/items/{productId}

Cambia la cantidad de un producto existente dentro del carrito.

Ejemplo:

```http
PUT /api/carts/{cartId}/items/{productId}
```

Body esperado:

```json
{
	"quantity": 3
}
```

Si el carrito y el producto existen, devuelve el carrito actualizado.

Si el carrito no existe, devuelve:

```http
404 Not Found
```

Si el producto no existe dentro del carrito, el dominio lanza una `DomainException` y el middleware global devuelve:

```http
400 Bad Request
```

## DELETE /api/carts/{cartId}/items/{productId}

Quita un producto específico del carrito.

Ejemplo:

```http
DELETE /api/carts/{cartId}/items/{productId}
```

Si el item existe, devuelve el carrito actualizado.

Si el producto no existe dentro del carrito, se devuelve un error de negocio controlado.

## DELETE /api/carts/{cartId}/items

Vacía el carrito.

Ejemplo:

```http
DELETE /api/carts/{cartId}/items
```

Elimina todos los items y devuelve el carrito actualizado.

## DELETE /api/carts/{id}

Elimina el carrito completo.

Ejemplo:

```http
DELETE /api/carts/{id}
```

Si el carrito existe, devuelve:

```http
204 No Content
```

Si el carrito no existe, devuelve:

```http
404 Not Found
```

## Relación con Application

`CartsController` no manipula directamente entidades del dominio ni usa Entity Framework Core.

El controller llama a:

```text
ICartService
```

La implementación concreta es:

```text
CartService
```

Esto permite mantener la API separada de la lógica de negocio.

## Relación con Domain

Las reglas principales del carrito siguen viviendo en el dominio, dentro de:

```text
Cart
CartItem
```

Por ejemplo:

- La cantidad debe ser mayor que cero.
- El precio unitario debe ser mayor que cero.
- Si se agrega dos veces el mismo producto, se incrementa la cantidad.
- No se puede quitar un producto inexistente del carrito.
- No se puede cambiar la cantidad de un producto inexistente.

## Relación con Infrastructure

La persistencia real está implementada en:

```text
CartRepository
```

Ese repositorio implementa:

```text
ICartRepository
```

y usa:

```text
AppDbContext
```

para guardar carritos e items en PostgreSQL.

## Flujo de agregar producto al carrito

El flujo completo es:

```text
Swagger / Cliente HTTP
        ↓
CartsController
        ↓
ICartService
        ↓
CartService
        ↓
Cart
        ↓
ICartRepository
        ↓
CartRepository
        ↓
AppDbContext
        ↓
PostgreSQL
```

## Manejo de errores

Los errores de negocio son lanzados como `DomainException`.

El middleware global los convierte en respuestas HTTP claras.

Ejemplo con cantidad inválida:

```json
{
	"productId": "f379aff5-d017-41ef-b944-603d3aed2c78",
	"productName": "Mouse Logitech",
	"unitPrice": 50,
	"quantity": 0
}
```

Respuesta esperada:

```http
400 Bad Request
```

```json
{
	"message": "La cantidad debe ser mayor que cero."
}
```

## Problema detectado durante pruebas

Durante las pruebas del endpoint:

```http
POST /api/carts/{cartId}/items
```

apareció una excepción de Entity Framework Core:

```text
DbUpdateConcurrencyException
```

El problema ocurrió porque al agregar un nuevo `CartItem`, EF Core podía interpretar el item como una entidad existente y tratar de actualizarla en lugar de insertarla.

## Solución aplicada

Se ajustó el método `UpdateAsync` de `CartRepository` para marcar explícitamente como `Added` los items nuevos que estén en estado `Detached`.

Ejemplo conceptual:

```csharp
foreach (var item in cart.Items)
{
    var entry = _context.Entry(item);

    if (entry.State == EntityState.Detached)
    {
        entry.State = EntityState.Added;
    }
}

await _context.SaveChangesAsync();
```

Con esto, EF Core puede insertar correctamente nuevos items del carrito.

## Pruebas realizadas

Se probaron desde Swagger:

- Crear carrito.
- Obtener carrito por Id.
- Agregar item al carrito.
- Validar que el carrito calcule `total` y `totalItems`.
- Verificar que se persistan items en PostgreSQL.
- Corregir el problema de tracking de `CartItem`.

## Resultado

Al finalizar este entregable, el sistema permite crear y administrar carritos reales mediante HTTP.

El carrito ya se persiste en PostgreSQL y puede almacenar items correctamente.
