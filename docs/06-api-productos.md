# API - Endpoints de productos

## Objetivo

En este entregable se expusieron endpoints HTTP para administrar productos desde la API.

El objetivo fue conectar la capa `Ecommerce.Api` con los casos de uso creados en `Ecommerce.Application` y con la persistencia real implementada en `Ecommerce.Infrastructure`.

Con esto, el sistema ya permite crear, consultar, actualizar y eliminar productos reales guardados en PostgreSQL.

## Controlador creado

Se creó el controlador:

```text
backend/src/Ecommerce.Api/Controllers/ProductsController.cs
```

Este controlador expone las operaciones principales del catálogo de productos.

## Configuración necesaria en Program.cs

Para que ASP.NET Core detecte los controladores, se agregó:

```csharp
builder.Services.AddControllers();
```

Y luego se mapearon los controladores con:

```csharp
app.MapControllers();
```

Además, la API registra las dependencias de Application e Infrastructure:

```csharp
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
```

Esto permite que `ProductsController` use `IProductService`, y que internamente se utilice `ProductRepository` con PostgreSQL.

## Endpoints creados

Se crearon los siguientes endpoints:

```http
GET    /api/products
GET    /api/products/{id}
POST   /api/products
PUT    /api/products/{id}
DELETE /api/products/{id}
```

## GET /api/products

Devuelve todos los productos registrados.

Ejemplo:

```http
GET /api/products
```

Si no hay productos cargados, devuelve una lista vacía:

```json
[]
```

Si existen productos, devuelve una lista con sus datos.

## GET /api/products/{id}

Devuelve un producto por su identificador.

Ejemplo:

```http
GET /api/products/ce39c0ca-fd12-4e0a-8fc0-d03588c1cee5
```

Si el producto existe, devuelve una respuesta `200 OK` con el producto:

```json
{
	"id": "ce39c0ca-fd12-4e0a-8fc0-d03588c1cee5",
	"name": "Notebook Lenovo",
	"description": "Notebook para trabajo y estudio.",
	"price": 1200,
	"stock": 5,
	"imageUrl": "https://example.com/notebook.jpg",
	"isActive": true,
	"createdAt": "2026-05-23T22:07:22.159654Z",
	"updatedAt": null
}
```

Si el producto no existe, devuelve:

```http
404 Not Found
```

Con una respuesta como:

```json
{
	"message": "Producto no encontrado."
}
```

## POST /api/products

Crea un producto nuevo.

Ejemplo:

```http
POST /api/products
```

Body esperado:

```json
{
	"name": "Notebook Lenovo",
	"description": "Notebook para trabajo y estudio.",
	"price": 1200,
	"stock": 5,
	"imageUrl": "https://example.com/notebook.jpg"
}
```

Respuesta esperada:

```http
201 Created
```

El endpoint devuelve el producto creado, incluyendo su `id`.

También agrega en los headers la ubicación del nuevo recurso mediante el header `Location`.

Ejemplo:

```text
location: http://localhost:5075/api/products/ce39c0ca-fd12-4e0a-8fc0-d03588c1cee5
```

## PUT /api/products/{id}

Actualiza los detalles principales de un producto existente.

Ejemplo:

```http
PUT /api/products/ce39c0ca-fd12-4e0a-8fc0-d03588c1cee5
```

Body esperado:

```json
{
	"name": "Notebook Lenovo Actualizada",
	"description": "Notebook actualizada para trabajo, estudio y programación.",
	"price": 1350,
	"imageUrl": "https://example.com/notebook-updated.jpg"
}
```

Este endpoint actualiza:

- Nombre.
- Descripción.
- Precio.
- Imagen.

No actualiza el stock, porque el stock tiene operaciones específicas separadas.

Si el producto existe, devuelve:

```http
200 OK
```

Si el producto no existe, devuelve:

```http
404 Not Found
```

## DELETE /api/products/{id}

Elimina un producto.

Ejemplo:

```http
DELETE /api/products/ce39c0ca-fd12-4e0a-8fc0-d03588c1cee5
```

Si el producto existe, devuelve:

```http
204 No Content
```

Si no existe, devuelve:

```http
404 Not Found
```

## Relación con Application

El controlador no trabaja directamente con Entity Framework Core ni con PostgreSQL.

En su lugar, depende de la interfaz:

```text
IProductService
```

Esto permite que la API dependa de casos de uso y no de detalles técnicos de infraestructura.

## Relación con Infrastructure

La implementación real de persistencia está en:

```text
ProductRepository
```

Ese repositorio implementa:

```text
IProductRepository
```

y usa:

```text
AppDbContext
```

para guardar los productos en PostgreSQL.

## Flujo de creación de producto

El flujo general al crear un producto es:

```text
Swagger / Cliente HTTP
        ↓
ProductsController
        ↓
IProductService
        ↓
ProductService
        ↓
Product
        ↓
IProductRepository
        ↓
ProductRepository
        ↓
AppDbContext
        ↓
PostgreSQL
```

## Pruebas realizadas

Se probaron los endpoints desde Swagger.

Pruebas realizadas correctamente:

- Crear producto con `POST /api/products`.
- Obtener producto por Id con `GET /api/products/{id}`.
- Listar productos con `GET /api/products`.
- Actualizar producto con `PUT /api/products/{id}`.
- Eliminar producto con `DELETE /api/products/{id}`.

## Consideración sobre errores de dominio

Si se envían datos inválidos, como precio igual a cero, stock negativo o nombre vacío, el dominio lanza una `DomainException`.

En el siguiente entregable se mejoró el manejo global de errores para convertir esas excepciones en respuestas HTTP más claras.

## Resultado

Al finalizar este entregable, la API permite administrar productos reales desde endpoints HTTP y persistirlos en PostgreSQL.

El sistema cuenta con el primer flujo completo funcionando:

```text
API → Application → Domain → Infrastructure → PostgreSQL
```
