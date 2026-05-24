# Carrito de compras - Dominio

## Objetivo

En este entregable se creó el dominio del carrito de compras.

El objetivo fue modelar las entidades `Cart` y `CartItem`, junto con sus reglas de negocio principales.

## Entidades creadas

Se crearon las entidades:

```text
backend/src/Ecommerce.Domain/Entities/Cart.cs
backend/src/Ecommerce.Domain/Entities/CartItem.cs
```

## Cart

`Cart` representa el carrito de compras de un usuario o sesión.

Contiene:

- Id.
- Items.
- CreatedAt.
- UpdatedAt.

Además expone propiedades calculadas:

- Total.
- TotalItems.
- IsEmpty.

## CartItem

`CartItem` representa un producto dentro del carrito.

Contiene:

- Id.
- ProductId.
- ProductName.
- UnitPrice.
- Quantity.

Además expone:

- Subtotal.

## Por qué CartItem guarda ProductName y UnitPrice

Cuando un producto se agrega al carrito, el carrito guarda una copia del nombre y del precio unitario.

Esto permite conservar una referencia del producto en el momento en que fue agregado.

Más adelante, esto será importante para las órdenes, porque una orden debe conservar el precio histórico de compra aunque el catálogo cambie después.

## Reglas de negocio

El dominio del carrito protege las siguientes reglas:

- El identificador del producto es obligatorio.
- El nombre del producto es obligatorio.
- El precio unitario debe ser mayor que cero.
- La cantidad debe ser mayor que cero.
- Si se agrega un producto que ya existe en el carrito, se incrementa su cantidad.
- No se puede eliminar un producto que no existe en el carrito.
- No se puede cambiar la cantidad de un producto que no existe en el carrito.

## Métodos principales de Cart

`Cart` expone métodos controlados para modificar su estado:

```csharp
cart.AddItem(...);
cart.RemoveItem(...);
cart.ChangeItemQuantity(...);
cart.Clear();
```

## Métodos principales de CartItem

`CartItem` expone métodos controlados para modificar la cantidad:

```csharp
item.IncreaseQuantity(...);
item.ChangeQuantity(...);
```

## Protección de la colección Items

La entidad `Cart` mantiene internamente una lista privada:

```csharp
private readonly List<CartItem> _items = new();
```

Y expone solo una colección de lectura:

```csharp
public IReadOnlyCollection<CartItem> Items => _items.AsReadOnly();
```

Esto evita que otras capas modifiquen el carrito directamente sin pasar por las reglas de negocio.

## Constructor de Cart

La entidad `Cart` mantiene un constructor privado para compatibilidad futura con Entity Framework Core.

También expone un constructor público para crear carritos desde el dominio:

```csharp
public Cart(Guid? id = null)
{
    Id = id ?? Guid.NewGuid();
    CreatedAt = DateTime.UtcNow;
}
```

Se usa un parámetro opcional para evitar duplicar constructores sin parámetros.

## Independencia del dominio

El carrito no depende de:

- ASP.NET Core.
- Entity Framework Core.
- PostgreSQL.
- Swagger.
- Angular.
- Azure.

Esto permite probar sus reglas de negocio de forma aislada.

## Tests

Se crearon tests para:

```text
backend/tests/Ecommerce.Tests/Domain/CartTests.cs
backend/tests/Ecommerce.Tests/Domain/CartItemTests.cs
```

Los tests verifican:

- Crear un carrito vacío.
- Agregar un producto.
- Agregar dos veces el mismo producto e incrementar cantidad.
- Evitar cantidades inválidas.
- Evitar precios inválidos.
- Quitar productos.
- Cambiar cantidades.
- Vaciar el carrito.
- Calcular totales.
- Crear items válidos.
- Validar datos inválidos en items.

## Resultado

Al finalizar este entregable, el sistema cuenta con el dominio base del carrito de compras, listo para ser conectado con Application, Infrastructure y API en entregables posteriores.
