# EcommerceDeployLab

## Objetivo del proyecto

EcommerceDeployLab es un proyecto educativo y profesional cuyo objetivo es construir, desplegar y documentar un sistema e-commerce full stack.

El propósito del proyecto no es solamente crear una aplicación web, sino aprender el ciclo completo de desarrollo de un sistema moderno:

- Análisis del problema.
- Diseño de arquitectura.
- Desarrollo backend.
- Desarrollo frontend.
- Persistencia de datos.
- Uso de base de datos relacional.
- Contenedores con Docker.
- Control de versiones con Git y GitHub.
- Despliegue en la nube.
- Automatización con CI/CD.
- Documentación técnica.
- Buenas prácticas de seguridad, configuración y monitoreo.

## Descripción general del sistema

El sistema será un e-commerce prototipo que permitirá:

- Visualizar productos.
- Administrar un catálogo.
- Manejar stock.
- Crear carritos de compra.
- Agregar productos al carrito.
- Validar productos contra el catálogo real.
- Realizar checkout.
- Generar órdenes de compra.
- Simular un pago.
- Registrar y administrar pagos.
- Confirmar una compra.
- Descontar stock al confirmar una orden.
- Ejecutar el checkout con transacciones.
- Guardar órdenes.
- Desplegar la solución completa en la nube.

El proyecto se construye por entregables pequeños, donde cada etapa agrega una funcionalidad concreta y documentada.

## Stack tecnológico inicial

El stack elegido para el proyecto es:

- Backend: ASP.NET Core Web API.
- Lenguaje backend: C#.
- Frontend: Angular.
- Base de datos: PostgreSQL.
- ORM: Entity Framework Core.
- Contenedores locales: Docker.
- Control de versiones: Git.
- Repositorio remoto: GitHub.
- CI/CD: GitHub Actions.
- Cloud: Azure.

## Arquitectura general

El backend se organiza siguiendo una estructura inspirada en Clean Architecture.

Los proyectos principales son:

- `Ecommerce.Api`
- `Ecommerce.Application`
- `Ecommerce.Domain`
- `Ecommerce.Infrastructure`
- `Ecommerce.Tests`

## Responsabilidad de cada proyecto

### Ecommerce.Domain

Contiene el núcleo del negocio.

Incluye:

- Entidades.
- Reglas de negocio.
- Excepciones de dominio.
- Enums.
- Comportamientos propios del sistema.

Actualmente contiene entidades como:

- `Product`
- `Cart`
- `CartItem`
- `Order`
- `OrderItem`
- `Payment`

No depende de base de datos, API, frameworks web ni infraestructura externa.

### Ecommerce.Application

Contiene los casos de uso de la aplicación.

Incluye:

- DTOs.
- Interfaces.
- Servicios de aplicación.
- Contratos que luego implementa Infrastructure.

Coordina acciones del sistema sin depender de detalles técnicos concretos.

Actualmente contiene casos de uso para:

- Productos.
- Carritos.
- Órdenes.
- Pagos.

Más adelante se agregarán casos de uso para:

- Checkout completo.

### Ecommerce.Infrastructure

Contiene implementaciones técnicas.

Incluye:

- Entity Framework Core.
- AppDbContext.
- Configuraciones de base de datos.
- Repositorios concretos.
- Acceso a PostgreSQL.
- Migraciones.

Actualmente implementa persistencia para:

- Productos.
- Carritos.
- Items del carrito.
- Órdenes.
- Items de órdenes.
- Pagos.

### Ecommerce.Api

Expone el sistema mediante HTTP.

Incluye:

- Controladores.
- Endpoints.
- Configuración de Swagger.
- Health checks.
- Registro de dependencias.
- Middleware global de errores.

Actualmente expone endpoints para:

- Productos.
- Operaciones de stock.
- Carritos.
- Items del carrito.
- Órdenes.
- Pagos.

### Ecommerce.Tests

Contiene pruebas automatizadas.

Incluye:

- Tests de dominio.
- Tests de aplicación.
- Pruebas unitarias de reglas y servicios.

Actualmente existen tests para:

- Productos.
- Operaciones de stock.
- Carritos.
- Items del carrito.
- Servicios de aplicación del carrito.
- Órdenes.
- Items de órdenes.
- Pagos.

## Flujo actual implementado

Hasta este punto, el sistema permite:

```text
Cliente HTTP / Swagger
        ↓
API Controllers
        ↓
Application Services
        ↓
Domain Entities
        ↓
Repositories
        ↓
Entity Framework Core
        ↓
PostgreSQL en Docker
```

Los productos pueden administrarse desde la API y persistirse en PostgreSQL.

Los carritos pueden crearse, consultarse y modificarse desde la API.

Al agregar un producto al carrito, el backend busca el producto real en el catálogo y toma de ahí:

- Nombre.
- Precio.
- Stock.
- Estado activo/inactivo.

Esto evita que el cliente pueda manipular el precio o el nombre del producto.

Las órdenes pueden crearse desde carritos reales.

Al confirmar una orden, el sistema valida los productos de la orden y descuenta el stock correspondiente del catálogo.

## Objetivo de la documentación

La carpeta `docs/` contiene documentación técnica del avance del proyecto.

Cada archivo describe un entregable o parte importante del sistema.

La carpeta `docs/adr/` contiene decisiones arquitectónicas. Estas decisiones explican por qué se eligió una tecnología, patrón o estructura determinada.

## Entregables documentados

Los entregables documentados hasta el momento son:

- `00-descripcion-general.md`
- `01-backend-base.md`
- `02-health-check-y-swagger.md`
- `03-catalogo-productos-dominio.md`
- `04-catalogo-productos-application.md`
- `05-infrastructure-efcore-postgresql.md`
- `06-api-productos.md`
- `07-manejo-global-errores.md`
- `08-operaciones-stock.md`
- `09-carrito-dominio.md`
- `10-carrito-application.md`
- `11-carrito-infrastructure.md`
- `12-carrito-api.md`
- `13-carrito-catalogo-real.md`
- `14-checkout-ordenes-dominio.md`
- `15-checkout-ordenes-application.md`
- `16-checkout-ordenes-infrastructure.md`
- `17-checkout-ordenes-api.md`
- `18-confirmacion-descuenta-stock.md`
- `19-transacciones-checkout.md`
- `20-pagos-dominio.md`
- `21-pagos-application.md`
- `22-pagos-infrastructure.md`
- `23-pagos-api.md`

## Decisiones arquitectónicas documentadas

Las decisiones arquitectónicas se encuentran en `docs/adr/`.

Hasta el momento se documentaron decisiones como:

- Usar arquitectura limpia.
- Proteger entidades de dominio.
- Usar DTOs y servicios de aplicación.
- Usar PostgreSQL con Entity Framework Core.
- Manejar errores mediante middleware global.
- Usar operaciones específicas para modificar stock.
- Diseñar el carrito como dominio propio.
- Definir casos de uso del carrito en Application.
- Persistir el carrito con Entity Framework Core.
- Exponer el carrito mediante controller.
- Integrar carrito con catálogo real.
- Modelar órdenes de compra en el dominio.
- Definir casos de uso de órdenes en Application.
- Persistir órdenes con Entity Framework Core.
- Exponer órdenes mediante controller.
- Descontar stock al confirmar una orden.
- Usar transacciones en checkout.
- Modelar pagos en el dominio.
- Definir casos de uso de pagos en Application.
- Persistir pagos con Entity Framework Core.
- Exponer pagos mediante controller.

## Estado actual del proyecto

Actualmente el proyecto cuenta con:

- Backend estructurado en capas.
- Catálogo de productos funcional.
- Persistencia en PostgreSQL.
- API de productos.
- Operaciones específicas de stock.
- Manejo global de errores.
- Carrito de compras con dominio, application, infrastructure y API.
- Integración del carrito con el catálogo real de productos.
- Dominio inicial de órdenes de compra.
- Casos de uso de órdenes en Application.
- Persistencia de órdenes con Entity Framework Core y PostgreSQL.
- API de órdenes para crear, consultar, confirmar y cancelar órdenes.
- Descuento de stock al confirmar órdenes.
- Confirmación de órdenes ejecutada dentro de una transacción.
- Dominio inicial de pagos.
- Casos de uso de pagos en Application.
- Persistencia de pagos con Entity Framework Core y PostgreSQL.
- API de pagos para registrar, consultar y modificar pagos.
- Tests unitarios para reglas de dominio y servicios de aplicación.
- Documentación técnica por entregable.
- ADRs para decisiones arquitectónicas importantes.

## Próximo paso

El siguiente paso es integrar pagos con el flujo de órdenes.

Ese entregable permitirá que aprobar un pago impacte en el estado del proceso de compra, dejando el checkout más cercano a un flujo real.
