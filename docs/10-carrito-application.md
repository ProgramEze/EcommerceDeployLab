# Carrito de compras - Application

## Objetivo

En este entregable se creó la capa `Application` para el carrito de compras.

El objetivo fue definir los casos de uso necesarios para operar con carritos sin depender todavía de la API, Entity Framework Core ni PostgreSQL.

Esta capa permite coordinar acciones del carrito usando las entidades del dominio `Cart` y `CartItem`, pero sin exponerlas directamente hacia futuras capas externas.

## Archivos creados

Se crearon DTOs, interfaces y servicios relacionados con el carrito.

## DTOs creados

```text
backend/src/Ecommerce.Application/DTOs/CartDto.cs
backend/src/Ecommerce.Application/DTOs/CartItemDto.cs
backend/src/Ecommerce.Application/DTOs/AddCartItemDto.cs
backend/src/Ecommerce.Application/DTOs/UpdateCartItemQuantityDto.cs
```

## CartDto

`CartDto` representa la información completa de un carrito que será devuelta por la aplicación.

Incluye:

- Id del carrito.
- Lista de items.
- Total del carrito.
- Cantidad total de productos.
- Indicador de si el carrito está vacío.
- Fecha de creación.
- Fecha de actualización.

Ejemplo conceptual:

```csharp
public class CartDto
{
    public Guid Id { get; set; }
    public IReadOnlyList<CartItemDto> Items { get; set; } = new List<CartItemDto>();
    public decimal Total { get; set; }
    public int TotalItems { get; set; }
    public bool IsEmpty { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
```

## CartItemDto

`CartItemDto` representa un producto dentro del carrito.

Incluye:

- Id del item.
- Id del producto.
- Nombre del producto.
- Precio unitario.
- Cantidad.
- Subtotal.

El subtotal se calcula a partir del precio unitario y la cantidad.

## AddCartItemDto

`AddCartItemDto` representa los datos necesarios para agregar un producto al carrito.

Incluye:

- ProductId.
- ProductName.
- UnitPrice.
- Quantity.

En esta etapa el DTO recibe el nombre y precio del producto directamente.

Más adelante, cuando el carrito esté conectado con productos reales desde la base de datos, se podrá mejorar el flujo para que el backend reciba solo el `ProductId` y la cantidad, y luego busque el nombre, precio y stock desde el catálogo.

## UpdateCartItemQuantityDto

`UpdateCartItemQuantityDto` representa los datos necesarios para cambiar la cantidad de un producto dentro del carrito.

Incluye:

- Quantity.

Esto permite modificar la cantidad de un item sin mezclar esta operación con otras acciones del carrito.

## Interfaces creadas

```text
backend/src/Ecommerce.Application/Interfaces/ICartRepository.cs
backend/src/Ecommerce.Application/Interfaces/ICartService.cs
```

## ICartRepository

`ICartRepository` define el contrato de persistencia que necesita la capa Application para trabajar con carritos.

Contiene operaciones como:

```csharp
Task<Cart?> GetByIdAsync(Guid id);
Task AddAsync(Cart cart);
Task UpdateAsync(Cart cart);
Task DeleteAsync(Cart cart);
```

## Por qué ICartRepository está en Application

La capa Application necesita guardar, buscar, actualizar y eliminar carritos, pero no debe saber cómo se guardan.

No debe depender de:

- PostgreSQL.
- Entity Framework Core.
- Redis.
- Archivos.
- Memoria.
- Servicios externos.

Por eso Application define una interfaz, y más adelante Infrastructure implementará esa interfaz usando la tecnología concreta que elijamos.

Esto respeta la idea de Clean Architecture: las capas internas definen contratos, y las capas externas implementan detalles.

## ICartService

`ICartService` define los casos de uso del carrito que luego podrán ser consumidos por la API.

Incluye operaciones como:

```csharp
Task<CartDto> CreateAsync();
Task<CartDto?> GetByIdAsync(Guid id);
Task<CartDto?> AddItemAsync(Guid cartId, AddCartItemDto dto);
Task<CartDto?> ChangeItemQuantityAsync(Guid cartId, Guid productId, UpdateCartItemQuantityDto dto);
Task<CartDto?> RemoveItemAsync(Guid cartId, Guid productId);
Task<CartDto?> ClearAsync(Guid cartId);
Task<bool> DeleteAsync(Guid cartId);
```

## Servicio creado

```text
backend/src/Ecommerce.Application/Services/CartService.cs
```

## Responsabilidad de CartService

`CartService` coordina los casos de uso relacionados con el carrito.

Permite:

- Crear un carrito.
- Obtener un carrito por Id.
- Agregar productos al carrito.
- Cambiar la cantidad de un producto.
- Quitar productos del carrito.
- Vaciar el carrito.
- Eliminar el carrito.

## Relación con Domain

`CartService` no contiene las reglas de negocio del carrito.

Las reglas siguen estando en las entidades del dominio:

```text
Cart
CartItem
```

Por ejemplo, las siguientes reglas pertenecen al dominio:

- La cantidad debe ser mayor que cero.
- El precio unitario debe ser mayor que cero.
- El identificador del producto es obligatorio.
- Si un producto ya existe en el carrito, se incrementa su cantidad.
- No se puede quitar un producto inexistente del carrito.
- No se puede cambiar la cantidad de un producto inexistente.

Application solo coordina el caso de uso.

## Flujo para agregar un producto al carrito

El flujo del caso de uso `AddItemAsync` es:

```text
Cliente / Futuro Controller
        ↓
CartService.AddItemAsync
        ↓
ICartRepository.GetByIdAsync
        ↓
Cart.AddItem
        ↓
ICartRepository.UpdateAsync
        ↓
CartDto
```

La regla de negocio de agregar o incrementar cantidad no está en el servicio, sino en `Cart`.

## Mapeo entre entidades y DTOs

`CartService` convierte entidades del dominio en DTOs.

Por ejemplo:

```text
Cart → CartDto
CartItem → CartItemDto
```

Esto evita devolver directamente entidades de dominio hacia la API o el frontend.

También permite cambiar la forma de respuesta sin modificar necesariamente el dominio.

## Registro de dependencias

Se registró `CartService` en:

```text
backend/src/Ecommerce.Application/DependencyInjection/DependencyInjection.cs
```

Con esta línea:

```csharp
services.AddScoped<ICartService, CartService>();
```

Esto permite que futuras capas, como la API, puedan solicitar `ICartService` mediante inyección de dependencias.

## Por qué todavía no registramos ICartRepository

En este entregable no se registra una implementación real de `ICartRepository`.

Eso se hará en el próximo entregable, cuando creemos la capa Infrastructure del carrito.

En este punto solo existe el contrato.

## Tests creados

Se crearon tests en:

```text
backend/tests/Ecommerce.Tests/Application/CartServiceTests.cs
```

## Qué verifican los tests

Los tests verifican que `CartService` pueda:

- Crear un carrito vacío.
- Obtener un carrito existente.
- Devolver `null` si el carrito no existe.
- Agregar un item al carrito.
- Incrementar cantidad si se agrega dos veces el mismo producto.
- Cambiar la cantidad de un item existente.
- Quitar un item existente.
- Vaciar un carrito.
- Eliminar un carrito.
- Devolver `false` al intentar eliminar un carrito inexistente.

## Repositorio falso en memoria

Para probar `CartService`, se creó un repositorio falso dentro de los tests:

```csharp
private class FakeCartRepository : ICartRepository
```

Este repositorio usa una lista en memoria para simular persistencia.

Esto permite probar la capa Application sin depender de PostgreSQL ni Entity Framework Core.

## Ventaja de esta decisión

Gracias a este enfoque, los tests son:

- Rápidos.
- Simples.
- Independientes de infraestructura.
- Fáciles de mantener.

Además, permiten validar los casos de uso antes de conectar el sistema con una base de datos real.

## Resultado

Al finalizar este entregable, el sistema cuenta con una capa Application para el carrito.

La aplicación ya tiene casos de uso listos para:

- Crear carritos.
- Consultar carritos.
- Agregar productos.
- Cambiar cantidades.
- Quitar productos.
- Vaciar carritos.
- Eliminar carritos.

El próximo paso será implementar la persistencia real del carrito en Infrastructure.
