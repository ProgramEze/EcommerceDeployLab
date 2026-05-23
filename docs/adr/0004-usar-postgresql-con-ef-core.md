# ADR 0004 - Usar PostgreSQL con Entity Framework Core

## Estado

Aceptado.

## Contexto

El proyecto necesita persistir datos reales para productos, carritos, órdenes y pagos.

Se requiere una base de datos relacional que funcione bien en desarrollo local, Docker y despliegue cloud.

## Decisión

Usaremos PostgreSQL como base de datos relacional y Entity Framework Core como ORM.

## Motivos

- PostgreSQL es una base de datos robusta y ampliamente usada.
- Funciona bien con Docker.
- Tiene soporte en Azure mediante Azure Database for PostgreSQL.
- Entity Framework Core permite trabajar con migraciones y LINQ.
- El stack se integra bien con ASP.NET Core.

## Consecuencias

- Infrastructure dependerá de EF Core y Npgsql.
- El equipo deberá manejar migraciones.
- La configuración de conexión será diferente entre desarrollo y producción.
- Las credenciales deberán manejarse fuera del código en ambientes reales.