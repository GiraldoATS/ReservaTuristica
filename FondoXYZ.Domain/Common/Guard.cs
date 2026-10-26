using FondoXYZ.Domain.Exceptions;

namespace FondoXYZ.Domain.Common;

/// <summary>
/// Validaciones reutilizables para proteger los invariantes de las entidades.
/// </summary>
internal static class Guard
{
    public static string Requerido(string? valor, string campo, int longitudMaxima = int.MaxValue)
    {
        if (string.IsNullOrWhiteSpace(valor))
            throw new DomainException($"El campo {campo} es obligatorio.");

        var limpio = valor.Trim();

        if (limpio.Length > longitudMaxima)
            throw new DomainException($"El campo {campo} no puede superar {longitudMaxima} caracteres.");

        return limpio;
    }

    public static int MayorQueCero(int valor, string campo)
    {
        if (valor <= 0)
            throw new DomainException($"El campo {campo} debe ser mayor a cero.");

        return valor;
    }

    public static int NoNegativo(int valor, string campo)
    {
        if (valor < 0)
            throw new DomainException($"El campo {campo} no puede ser negativo.");

        return valor;
    }

    public static decimal MayorQueCero(decimal valor, string campo)
    {
        if (valor <= 0)
            throw new DomainException($"El campo {campo} debe ser mayor a cero.");

        return valor;
    }

    public static decimal NoNegativo(decimal valor, string campo)
    {
        if (valor < 0)
            throw new DomainException($"El campo {campo} no puede ser negativo.");

        return valor;
    }
}
