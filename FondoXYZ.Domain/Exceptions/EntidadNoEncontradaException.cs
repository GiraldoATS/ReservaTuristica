namespace FondoXYZ.Domain.Exceptions;

public sealed class EntidadNoEncontradaException : DomainException
{
    public EntidadNoEncontradaException(string entidad, object id)
        : base($"{entidad} con id '{id}' no existe.")
    {
    }
}
