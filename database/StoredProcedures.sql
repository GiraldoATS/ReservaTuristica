-- Procedimientos almacenados usados por los adaptadores de FondoXYZ.Infrastructure
-- (Persistence/Queries). Se versionan aquí para poder recrear la base de datos.

CREATE OR ALTER PROCEDURE [dbo].[sp_ConsultarDisponibilidad]
(
    @FechaInicio DATE,
    @FechaFin DATE
)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        A.IdAlojamiento,
        S.Nombre AS Sede,
        A.Nombre AS Alojamiento,
        A.CapacidadMaxima,
        A.Tipo
    FROM Alojamientos A
    INNER JOIN Sedes S
        ON A.IdSede = S.IdSede
    WHERE
        A.Estado = 1
        AND A.IdAlojamiento NOT IN
        (
            SELECT R.IdAlojamiento
            FROM Reservas R
            WHERE
                @FechaInicio BETWEEN R.FechaInicio AND R.FechaFin
                OR @FechaFin BETWEEN R.FechaInicio AND R.FechaFin
                OR R.FechaInicio BETWEEN @FechaInicio AND @FechaFin
        )
END
GO

CREATE OR ALTER PROCEDURE [dbo].[sp_CalcularTarifa]
(
    @IdAlojamiento INT,
    @FechaInicio DATE,
    @FechaFin DATE,
    @NumeroPersonas INT
)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @IdTemporada INT
    DECLARE @PrecioBase DECIMAL(18,2)
    DECLARE @PrecioPersonaAdicional DECIMAL(18,2)
    DECLARE @NumeroNoches INT
    DECLARE @PersonasAdicionales INT
    DECLARE @Total DECIMAL(18,2)

    SET @NumeroNoches = DATEDIFF(DAY, @FechaInicio, @FechaFin)

    SELECT TOP 1
        @IdTemporada = IdTemporada
    FROM Temporadas
    WHERE
        @FechaInicio BETWEEN FechaInicio AND FechaFin
        AND Estado = 1

    SELECT TOP 1
        @PrecioBase = PrecioBase,
        @PrecioPersonaAdicional = PrecioPersonaAdicional
    FROM Tarifas
    WHERE
        IdAlojamiento = @IdAlojamiento
        AND IdTemporada = @IdTemporada
        AND Estado = 1

    -- La tarifa base incluye hasta 4 personas.
    SET @PersonasAdicionales =
        CASE
            WHEN @NumeroPersonas > 4 THEN @NumeroPersonas - 4
            ELSE 0
        END

    SET @Total =
        (@PrecioBase + (@PersonasAdicionales * @PrecioPersonaAdicional)) * @NumeroNoches

    -- Si no hay temporada o tarifa vigente, las columnas se devuelven en NULL.
    SELECT
        @IdTemporada AS IdTemporada,
        @PrecioBase AS PrecioBase,
        @PrecioPersonaAdicional AS PrecioPersonaAdicional,
        @NumeroNoches AS NumeroNoches,
        @PersonasAdicionales AS PersonasAdicionales,
        @Total AS TotalPagar
END
GO
