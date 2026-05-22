# EcommerceDeployLab

## Objetivo del proyecto

EcommerceDeployLab es un proyecto educativo y profesional cuyo objetivo es construir, desplegar y documentar un sistema e-commerce full stack.

El sistema permitirá aprender no solo programación, sino también arquitectura, infraestructura cloud, base de datos, CI/CD, seguridad, monitoreo y buenas prácticas de despliegue.

## Stack tecnológico inicial

- Backend: ASP.NET Core Web API
- Frontend: Angular
- Base de datos: PostgreSQL
- ORM: Entity Framework Core
- Cloud: Azure
- Repositorio: GitHub
- CI/CD: GitHub Actions

## Arquitectura inicial

El backend se divide en cuatro proyectos principales:

- Ecommerce.Api
- Ecommerce.Application
- Ecommerce.Domain
- Ecommerce.Infrastructure

## Primeros endpoints técnicos

- GET /
- GET /health

## Motivo del endpoint /health

El endpoint `/health` permite verificar si la API está viva. Más adelante será útil para monitoreo, despliegues, pipelines y diagnóstico en Azure.