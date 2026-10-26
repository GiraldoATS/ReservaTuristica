namespace FondoXYZ.Application.Common;

/// <summary>
/// Resultado de una operación que puede fallar por motivos esperados
/// (por ejemplo, validaciones de Identity), sin recurrir a excepciones.
/// </summary>
public sealed record Result(bool Exitoso, string Error)
{
    public static Result Ok() => new(true, string.Empty);

    public static Result Fallo(string error) => new(false, error);
}
