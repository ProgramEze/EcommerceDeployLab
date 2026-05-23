# Catálogo de productos - Dominio

## Objetivo

En este entregable se creó la entidad principal del catálogo: `Product`.

La entidad representa un producto vendible dentro del e-commerce y contiene las primeras reglas de negocio reales del sistema.

## Ubicación

La entidad se encuentra en:

```text
backend/src/Ecommerce.Domain/Entities/Product.cs
```

La excepción de dominio se encuentra en:

```text
backend/src/Ecommerce.Domain/Exceptions/DomainException.cs
```

## Entidad Product

La clase `Product` representa un producto del catálogo.

Contiene información como:

- Identificador.
- Nombre.
- Descripción.
- Precio.
- Stock.
- URL de imagen.
- Estado activo o inactivo.
- Fecha de creación.
- Fecha de actualización.

## Responsabilidad de Product

La clase `Product` contiene datos y comportamientos propios de un producto.

Permite:

- Crear un producto válido.
- Actualizar nombre, descripción, precio e imagen.
- Incrementar stock.
- Descontar stock.
- Activar producto.
- Desactivar producto.

## Reglas de negocio

La entidad protege las siguientes reglas:

- El nombre del producto es obligatorio.
- El nombre no puede superar los 100 caracteres.
- La descripción es obligatoria.
- La descripción no puede superar los 500 caracteres.
- El precio debe ser mayor que cero.
- El stock no puede ser negativo.
- No se puede descontar más stock del disponible.
- Las cantidades para incrementar o descontar stock deben ser mayores que cero.

## Por qué usamos private set

Las propiedades de `Product` usan `private set` para evitar que el estado de la entidad sea modificado directamente desde afuera.

Esto evita estados inválidos como:

```csharp
product.Price = -100;
product.Stock = -5;
```

En cambio, obligamos a modificar el producto mediante métodos controlados:

```csharp
product.UpdateDetails(...);
product.IncreaseStock(...);
product.DecreaseStock(...);
product.Activate();
product.Deactivate();
```

De esta forma, la entidad mantiene sus reglas internas y protege la consistencia del negocio.

## Por qué usamos DomainException

`DomainException` representa errores propios del negocio.

Por ejemplo:

- El nombre del producto es obligatorio.
- El precio debe ser mayor que cero.
- No hay stock suficiente.

Esto permite diferenciar errores de negocio de errores técnicos.

## Independencia del dominio

La entidad `Product` no depende de:

- Entity Framework Core.
- PostgreSQL.
- ASP.NET Core.
- Swagger.
- Azure.
- Angular.

Esto permite que el dominio pueda probarse de forma aislada.

## Tests

Los tests de dominio están en:

```text
backend/tests/Ecommerce.Tests/Domain/ProductTests.cs
```

Estos tests verifican que:

- Un producto válido se crea correctamente.
- Un producto inválido lanza `DomainException`.
- El stock se incrementa correctamente.
- El stock se descuenta correctamente.
- No se puede descontar más stock del disponible.
- El producto puede activarse y desactivarse.
- Los detalles del producto pueden actualizarse correctamente.

## Importancia de este entregable

Este entregable es importante porque crea la primera pieza real del núcleo del sistema.

Antes de pensar en base de datos, API o frontend, el dominio define qué es un producto válido y qué operaciones se pueden realizar sobre él.

## Resultado

Al finalizar este entregable, el sistema cuenta con una entidad `Product` robusta, validada y protegida mediante reglas de negocio.