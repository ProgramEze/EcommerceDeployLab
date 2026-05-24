# Carrito integrado con catálogo real

## Objetivo

En este entregable se integró el carrito de compras con el catálogo real de productos.

El objetivo fue evitar que el cliente envíe datos sensibles o manipulables como el nombre y el precio del producto.

Antes de este entregable, para agregar un producto al carrito se enviaba:

```json
{
	"productId": "guid-del-producto",
	"productName": "Mouse Logitech",
	"unitPrice": 50,
	"quantity": 5
}
```

Después de este entregable, el cliente solo envía:

```json
{
	"productId": "guid-del-producto",
	"quantity": 5
}
```

El backend busca el producto real en PostgreSQL y toma de ahí el nombre, precio, stock y estado.

## Archivos modificados

Se modificó:

```text
backend/src/Ecommerce.Application/DTOs/AddCartItemDto.cs
backend/src/Ecommerce.Application/Services/CartService.cs
backend/tests/Ecommerce.Tests/Application/CartServiceTests.cs
```

## Cambio en AddCartItemDto

`AddCartItemDto` ahora contiene solo:

```csharp
public Guid ProductId { get; set; }
public int Quantity { get; set; }
```

Se eliminaron:

```text
ProductName
UnitPrice
```

## Motivo del cambio

El cliente no debería definir el nombre ni el precio del producto.

Si el cliente pudiera enviar `unitPrice`, podría intentar agregar un producto con un precio falso.

Por ejemplo:

```json
{
	"productId": "guid-del-producto",
	"unitPrice": 1,
	"quantity": 5
}
```

Esto sería inseguro.

## Nueva responsabilidad de CartService

`CartService` ahora depende de:

```text
ICartRepository
IProductRepository
```

Esto permite que, al agregar un producto al carrito, el servicio pueda:

- Buscar el carrito.
- Buscar el producto real.
- Validar que el producto exista.
- Validar que el producto esté activo.
- Validar stock suficiente.
- Tomar el nombre real del producto.
- Tomar el precio real del producto.
- Agregar el item al carrito.

## Validaciones agregadas

Se agregaron las siguientes validaciones:

- El producto debe existir.
- El producto debe estar activo.
- La cantidad solicitada no puede superar el stock disponible.
- Si el producto ya está en el carrito, se valida la suma entre la cantidad actual y la nueva cantidad.

## Flujo actualizado

```text
Cliente / Swagger
        ↓
CartsController
        ↓
CartService
        ↓
IProductRepository
        ↓
ProductRepository
        ↓
Product
        ↓
Cart.AddItem
        ↓
ICartRepository
        ↓
CartRepository
        ↓
PostgreSQL
```

## Ejemplo actual de request

```http
POST /api/carts/{cartId}/items
```

Body:

```json
{
	"productId": "f379aff5-d017-41ef-b944-603d3aed2c78",
	"quantity": 5
}
```

## Ejemplo de respuesta

```json
{
	"items": [
		{
			"productId": "f379aff5-d017-41ef-b944-603d3aed2c78",
			"productName": "Mouse Logitech",
			"unitPrice": 50,
			"quantity": 5,
			"subtotal": 250
		}
	],
	"total": 250,
	"totalItems": 5,
	"isEmpty": false
}
```

## Tests

Se actualizaron los tests de `CartService` para usar un repositorio falso de productos.

También se agregaron pruebas para validar:

- Producto inexistente.
- Producto inactivo.
- Stock insuficiente.
- Stock insuficiente considerando cantidades ya existentes en el carrito.
- Uso del nombre y precio reales del producto.

## Resultado

Al finalizar este entregable, el carrito queda integrado con el catálogo real.

El sistema es más seguro porque el precio y nombre del producto ya no dependen del cliente.
