namespace FondoXYZ.Domain.Exceptions;

/// <summary>
/// Se lanza cuando una operación viola una regla de negocio.
/// Su mensaje está pensado para mostrarse al usuario final.
/// </summary>
public class DomainException : Exception
{
    public DomainException(string message) : base(message)
    {
    }
}
