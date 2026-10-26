namespace FondoXYZ.Application.DTOs;

public sealed record LoginCommand(
    string Email,
    string Password,
    bool Recordarme);

public sealed record RegistrarUsuarioCommand(
    string NombreCompleto,
    string Email,
    string Password);

public sealed record RestablecerPasswordCommand(
    string Email,
    string Token,
    string NuevaPassword);
