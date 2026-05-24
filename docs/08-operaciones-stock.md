# Operaciones específicas de stock

## Objetivo

En este entregable se agregaron operaciones específicas para incrementar y descontar stock de productos.

El objetivo fue evitar que el stock se modifique como un dato común dentro de la actualización general del producto.

## Problema detectado

El endpoint `PUT /api/products/{id}` actualiza datos descriptivos del producto, como:

- Nombre.
- Descripción.
- Precio.
- Imagen.

Pero el stock tiene reglas propias y representa disponibilidad real para vender.

Por eso no conviene modificarlo como parte de una actualización general.

## Endpoints creados

Se agregaron los siguientes endpoints:

```http
PATCH /api/products/{id}/stock/increase
PATCH /api/products/{id}/stock/decrease
```

## DTO creado

Se creó el DTO:

```text
backend/src/Ecommerce.Application/DTOs/UpdateStockDto.cs
```

Con la propiedad:

```csharp
public int Quantity { get; set; }
```

Este DTO representa la cantidad a incrementar o descontar.

## Casos de uso agregados

En `IProductService` y `ProductService` se agregaron:

```csharp
IncreaseStockAsync(Guid id, UpdateStockDto dto)
DecreaseStockAsync(Guid id, UpdateStockDto dto)
```

Estos métodos buscan el producto, aplican la operación correspondiente y guardan los cambios mediante `IProductRepository`.

## Métodos del dominio utilizados

La lógica real de stock sigue viviendo en la entidad `Product`.

Se reutilizan los métodos:

```csharp
product.IncreaseStock(quantity);
product.DecreaseStock(quantity);
```

Esto evita duplicar reglas en Application o API.

## Reglas de negocio

Las reglas de stock aplicadas son:

- La cantidad a incrementar debe ser mayor que cero.
- La cantidad a descontar debe ser mayor que cero.
- No se puede descontar más stock del disponible.
- El stock no puede quedar negativo.

## PATCH /api/products/{id}/stock/increase

Incrementa el stock de un producto.

Ejemplo:

```http
PATCH /api/products/{id}/stock/increase
```

Body:

```json
{
	"quantity": 5
}
```

Si el producto tenía stock `10`, después de esta operación pasa a tener stock `15`.

## PATCH /api/products/{id}/stock/decrease

Descuenta stock de un producto.

Ejemplo:

```http
PATCH /api/products/{id}/stock/decrease
```

Body:

```json
{
	"quantity": 3
}
```

Si el producto tenía stock `15`, después de esta operación pasa a tener stock `12`.

## Errores esperados

Si se intenta incrementar o descontar una cantidad inválida:

```json
{
	"quantity": 0
}
```

La API devuelve:

```http
400 Bad Request
```

Con un mensaje como:

```json
{
	"message": "La cantidad a descontar debe ser mayor que cero."
}
```

Si se intenta descontar más stock del disponible, la API devuelve:

```json
{
	"message": "No hay stock suficiente para realizar esta operación."
}
```

## Por qué el stock tiene endpoints propios

El stock representa una regla importante del negocio.

No conviene actualizarlo dentro de un `PUT` general del producto porque más adelante el stock puede cambiar por distintos motivos:

- Compra confirmada.
- Cancelación de compra.
- Reposición manual.
- Ajuste administrativo.
- Devoluciones.

Separar estas operaciones hace que el sistema sea más claro y seguro.

## Pruebas realizadas

Se probaron desde Swagger:

- Incrementar stock.
- Descontar stock.
- Intentar incrementar con cantidad inválida.
- Intentar descontar con cantidad inválida.
- Intentar descontar más stock del disponible.

## Resultado

Al finalizar este entregable, la API permite manejar el stock mediante operaciones específicas y respetando las reglas del dominio.

El módulo de productos queda más robusto y preparado para integrarse con carrito, órdenes y checkout.
