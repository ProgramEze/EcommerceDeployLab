# Catálogo de productos - Application

## Objetivo

En este entregable se creó la capa de aplicación para el catálogo de productos.

Esta capa contiene los casos de uso principales relacionados con productos.

## Ubicación

Los archivos principales se encuentran en:

```text
backend/src/Ecommerce.Application/DTOs
backend/src/Ecommerce.Application/Interfaces
backend/src/Ecommerce.Application/Services
```

## Archivos creados

Se crearon los siguientes archivos:

```text
ProductDto.cs
CreateProductDto.cs
UpdateProductDto.cs
IProductRepository.cs
IProductService.cs
ProductService.cs
```

## Responsabilidad de Application

La capa Application coordina los casos de uso del sistema.

En el módulo de productos permite:

- Listar productos.
- Buscar un producto por Id.
- Crear un producto.
- Actualizar un producto.
- Eliminar un producto.

## DTOs creados

### ProductDto

Representa los datos que la aplicación devuelve al consultar productos.

Incluye:

- Id.
- Name.
- Description.
- Price.
- Stock.
- ImageUrl.
- IsActive.
- CreatedAt.
- UpdatedAt.

### CreateProductDto

Representa los datos necesarios para crear un producto.

Incluye:

- Name.
- Description.
- Price.
- Stock.
- ImageUrl.

### UpdateProductDto

Representa los datos necesarios para actualizar los detalles principales de un producto.

Incluye:

- Name.
- Description.
- Price.
- ImageUrl.

No incluye `Stock`, porque el stock tendrá operaciones específicas.

## Por qué usamos DTOs

Los DTOs permiten separar el modelo interno del dominio de los datos que entran o salen de la aplicación.

Esto evita exponer directamente las entidades del dominio hacia la API o el frontend.

También permite que el contrato externo pueda cambiar sin modificar necesariamente el dominio.

## IProductRepository

`IProductRepository` define lo que la aplicación necesita para persistir productos.

La interfaz contiene operaciones como:

- Obtener todos los productos.
- Obtener un producto por Id.
- Agregar un producto.
- Actualizar un producto.
- Eliminar un producto.

## Por qué IProductRepository está en Application

Application necesita persistir datos, pero no debe depender de una tecnología concreta.

Por eso define el contrato, pero no la implementación.

La implementación real estará en Infrastructure.

Esto permite que Application no dependa de:

- Entity Framework Core.
- PostgreSQL.
- SQL.
- Archivos.
- Servicios externos.

## IProductService

`IProductService` define los casos de uso que luego la API podrá consumir.

La API no debería conocer detalles internos de cómo se crean o modifican los productos.

## ProductService

`ProductService` implementa los casos de uso de productos.

Sus responsabilidades son:

- Recibir DTOs.
- Crear entidades del dominio.
- Usar `IProductRepository`.
- Mapear entidades a DTOs.
- Coordinar operaciones de aplicación.

## Relación entre capas

La relación queda así:

```text
Api → Application → Domain
Infrastructure → Application
Infrastructure → Domain
```

La capa Application no sabe cómo se guardan los productos. Solo sabe que existe un contrato llamado `IProductRepository`.

## Tests

Los tests de esta capa están en:

```text
backend/tests/Ecommerce.Tests/Application/ProductServiceTests.cs
```

Estos tests usan un repositorio falso en memoria para verificar el comportamiento de `ProductService` sin depender de una base de datos real.

## Resultado

Al finalizar este entregable, el sistema quedó con una capa Application capaz de coordinar operaciones de productos sin depender todavía de infraestructura real.