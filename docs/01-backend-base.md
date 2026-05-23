# Backend base

## Objetivo

En este entregable se creó la estructura inicial del backend del proyecto EcommerceDeployLab.

El objetivo fue preparar una solución ASP.NET Core organizada en capas, lista para crecer de forma mantenible.

## Estructura creada

La estructura inicial del backend es:

```text
backend/
  EcommerceDeployLab.sln
  src/
    Ecommerce.Api/
    Ecommerce.Application/
    Ecommerce.Domain/
    Ecommerce.Infrastructure/
  tests/
    Ecommerce.Tests/
```

## Proyectos creados

### Ecommerce.Api

Proyecto ASP.NET Core Web API.

Responsabilidades:

- Exponer endpoints HTTP.
- Configurar Swagger.
- Configurar middlewares.
- Registrar dependencias.
- Servir como punto de entrada de la aplicación.

### Ecommerce.Domain

Proyecto de librería de clases.

Responsabilidades:

- Contener entidades.
- Contener reglas de negocio.
- Mantener el núcleo del sistema independiente de frameworks externos.

### Ecommerce.Application

Proyecto de librería de clases.

Responsabilidades:

- Contener casos de uso.
- Definir interfaces.
- Trabajar con DTOs.
- Coordinar operaciones de aplicación.

### Ecommerce.Infrastructure

Proyecto de librería de clases.

Responsabilidades:

- Implementar acceso a datos.
- Configurar Entity Framework Core.
- Implementar repositorios.
- Conectar con servicios externos o infraestructura técnica.

### Ecommerce.Tests

Proyecto de pruebas con xUnit.

Responsabilidades:

- Probar reglas del dominio.
- Probar servicios de aplicación.
- Validar comportamiento del sistema sin depender necesariamente de infraestructura real.

## Comandos principales utilizados

Se creó la solución con:

```powershell
dotnet new sln -n EcommerceDeployLab
```

Se crearon los proyectos principales:

```powershell
dotnet new webapi -n Ecommerce.Api -o src/Ecommerce.Api
dotnet new classlib -n Ecommerce.Domain -o src/Ecommerce.Domain
dotnet new classlib -n Ecommerce.Application -o src/Ecommerce.Application
dotnet new classlib -n Ecommerce.Infrastructure -o src/Ecommerce.Infrastructure
dotnet new xunit -n Ecommerce.Tests -o tests/Ecommerce.Tests
```

Se agregaron los proyectos a la solución:

```powershell
dotnet sln add src/Ecommerce.Api/Ecommerce.Api.csproj
dotnet sln add src/Ecommerce.Domain/Ecommerce.Domain.csproj
dotnet sln add src/Ecommerce.Application/Ecommerce.Application.csproj
dotnet sln add src/Ecommerce.Infrastructure/Ecommerce.Infrastructure.csproj
dotnet sln add tests/Ecommerce.Tests/Ecommerce.Tests.csproj
```

## Referencias entre proyectos

La dirección de dependencias quedó definida así:

```text
Ecommerce.Api
  depende de Application
  depende de Infrastructure

Ecommerce.Infrastructure
  depende de Application
  depende de Domain

Ecommerce.Application
  depende de Domain

Ecommerce.Domain
  no depende de nadie
```

## Motivo de esta estructura

La estructura permite separar responsabilidades.

El dominio no debe depender de la API, de Entity Framework, de PostgreSQL ni de Azure.

Esto permite que las reglas principales del negocio sean más fáciles de probar, mantener y reutilizar.

## Verificación

Se verificó que la solución compilara correctamente con:

```powershell
dotnet build
```

También se ejecutó la API con:

```powershell
dotnet run --project src/Ecommerce.Api
```

## Control de versiones

Se inicializó Git en el proyecto y se creó el primer commit con la estructura base del backend.

Commit sugerido:

```text
Create backend solution structure
```

## Resultado

Al finalizar este entregable, el backend quedó creado con una arquitectura base preparada para crecer de forma ordenada.