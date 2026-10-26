# FondoXYZ – Reservas turísticas

Aplicación ASP.NET Core MVC (.NET 8) para gestionar sedes, alojamientos, tarifas por temporada y reservas.

## Arquitectura hexagonal (puertos y adaptadores)

```
                 ┌──────────────────────────────────────────────┐
  Adaptador de   │                 Application                  │   Adaptadores de salida
  entrada        │  Ports/In  (casos de uso: IReservaService…)  │   (FondoXYZ.Infrastructure)
                 │  Services  (implementación de casos de uso)  │
 FondoXYZ.Web ──►│  Ports/Out (IReservaRepository, IUnitOfWork, │◄── EF Core + SQL Server
 (MVC)           │             ICalculoTarifaQuery, IIdentity…) │◄── Procedimientos almacenados
                 │  ┌────────────────────────────────────────┐  │◄── ASP.NET Core Identity
                 │  │                Domain                  │  │◄── SMTP
                 │  │ Entidades, RangoFechas, reglas negocio │  │
                 │  └────────────────────────────────────────┘  │
                 └──────────────────────────────────────────────┘
```

| Proyecto | Responsabilidad | Depende de |
|---|---|---|
| `FondoXYZ.Domain` | Entidades con comportamiento (`Reserva.Crear`, `Alojamiento.ValidarDisponibleParaReserva`), value object `RangoFechas`, excepciones de dominio. | Nada (ningún paquete NuGet) |
| `FondoXYZ.Application` | Casos de uso, DTOs/commands, **puertos de entrada** (`Ports/In`) y **puertos de salida** (`Ports/Out`). | Domain |
| `FondoXYZ.Infrastructure` | **Adaptadores de salida**: `ApplicationDbContext` (también `IUnitOfWork`), repositorios EF, consultas sobre SPs, Identity, correo SMTP, migraciones. | Application |
| `FondoXYZ.Web` | **Adaptador de entrada** MVC: controladores delgados, ViewModels con validación de formularios. Es el *composition root*. | Application (+ Infrastructure solo para el registro DI) |
| `FondoXYZ.Tests` | Pruebas unitarias xUnit del dominio y de los casos de uso con fakes en memoria. | Application |

La regla de dependencias siempre apunta hacia el núcleo: el dominio no conoce EF, Identity ni MVC.

## Principios SOLID aplicados

- **S – Responsabilidad única**: el envío SMTP (`SmtpEmailSender`) está separado de la plantilla HTML (`PlantillasCorreo`) y del caso de uso (`AuthService`); cada entidad tiene su `IEntityTypeConfiguration`; los controladores solo traducen HTTP ⇄ casos de uso.
- **O – Abierto/cerrado**: cambiar SQL Server por otra base, o el SMTP por SendGrid, es escribir un nuevo adaptador sin tocar Application ni Domain.
- **L – Sustitución de Liskov**: cualquier implementación de un puerto (EF real o fake de pruebas) es intercambiable; las pruebas lo demuestran.
- **I – Segregación de interfaces**: repositorios (escritura/entidades) separados de consultas de solo lectura (`IDisponibilidadQuery`, `ICalculoTarifaQuery`, `IDashboardQuery`).
- **D – Inversión de dependencias**: Application define los puertos y la infraestructura los implementa; también se inyecta `TimeProvider` en lugar de usar `DateTime.Now`, lo que hace que la lógica de fechas sea testeable.

## Decisiones de diseño

- **Modelo de dominio rico**: setters privados y métodos de fábrica (`Crear`/`Actualizar`) que validan invariantes; no se puede crear una entidad en estado inválido.
- **Sin over-posting**: los formularios se enlazan a ViewModels, nunca a entidades.
- **Unit of Work**: los repositorios registran cambios y el caso de uso confirma una sola vez.
- **El total de la reserva se calcula siempre en el servidor**; el valor del formulario es solo informativo.
- **Errores de negocio** como `DomainException`, que los controladores muestran como mensajes de validación (sin `catch (Exception)` genéricos).
- Los procedimientos almacenados están versionados en [`database/StoredProcedures.sql`](database/StoredProcedures.sql).

## Ejecución

```bash
dotnet user-secrets set "EmailSettings:Username" "<correo>" --project FondoXYZ.Web
dotnet user-secrets set "EmailSettings:Password" "<app-password>" --project FondoXYZ.Web
dotnet run --project FondoXYZ.Web
dotnet test
```
