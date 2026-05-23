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
- Agregar productos a un carrito.
- Realizar un checkout.
- Simular un pago.
- Confirmar una compra.
- Guardar órdenes.
- Desplegar la solución completa en la nube.

El proyecto se construirá por entregables pequeños, donde cada etapa agrega una funcionalidad concreta y documentada.

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
- Comportamientos propios del sistema.

No depende de base de datos, API, frameworks web ni infraestructura externa.

### Ecommerce.Application

Contiene los casos de uso de la aplicación.

Incluye:

- DTOs.
- Interfaces.
- Servicios de aplicación.
- Contratos que luego implementará Infrastructure.

Coordina acciones del sistema sin depender de detalles técnicos concretos.

### Ecommerce.Infrastructure

Contiene implementaciones técnicas.

Incluye:

- Entity Framework Core.
- AppDbContext.
- Configuraciones de base de datos.
- Repositorios concretos.
- Acceso a PostgreSQL.

### Ecommerce.Api

Expone el sistema mediante HTTP.

Incluye:

- Controladores.
- Endpoints.
- Configuración de Swagger.
- Health checks.
- Registro de dependencias.

### Ecommerce.Tests

Contiene pruebas automatizadas.

Incluye:

- Tests de dominio.
- Tests de aplicación.
- Pruebas unitarias de reglas y servicios.

## Objetivo de la documentación

La carpeta `docs/` contiene documentación técnica del avance del proyecto.

Cada archivo describe un entregable o parte importante del sistema.

La carpeta `docs/adr/` contiene decisiones arquitectónicas. Estas decisiones explican por qué se eligió una tecnología, patrón o estructura determinada.

## Entregables documentados

Los entregables iniciales son:

- Backend base.
- Health check y Swagger.
- Dominio del catálogo de productos.
- Capa Application para productos.
- Infrastructure con EF Core y PostgreSQL.
- API de productos.

## Estado actual del proyecto

Actualmente el proyecto cuenta con una estructura backend inicial, documentación técnica, arquitectura por capas, dominio de productos, capa de aplicación e infraestructura con PostgreSQL.

El siguiente paso es exponer endpoints HTTP para administrar productos desde la API.