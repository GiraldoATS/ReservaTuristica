using Microsoft.AspNetCore.Identity;

namespace FondoXYZ.Infrastructure.Identity;

public class ApplicationUser : IdentityUser
{
    public const int LongitudMaximaNombre = 150;

    public string NombreCompleto { get; set; } = string.Empty;

    public DateTime FechaCreacion { get; set; } = DateTime.Now;
}
