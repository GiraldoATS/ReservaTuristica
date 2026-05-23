using Microsoft.AspNetCore.Identity;

namespace FondoXYZ.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string NombreCompleto { get; set; }
            = string.Empty;

        public DateTime FechaCreacion { get; set; }
            = DateTime.Now;
    }
}