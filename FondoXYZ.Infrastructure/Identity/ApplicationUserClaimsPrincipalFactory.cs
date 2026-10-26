using System.Security.Claims;
using FondoXYZ.Application.Common;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace FondoXYZ.Infrastructure.Identity;

/// <summary>
/// Agrega el nombre completo a la cookie de autenticación, para que las vistas
/// lo muestren sin consultar la base de datos en cada request.
/// </summary>
internal sealed class ApplicationUserClaimsPrincipalFactory
    : UserClaimsPrincipalFactory<ApplicationUser, IdentityRole>
{
    public ApplicationUserClaimsPrincipalFactory(
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager,
        IOptions<IdentityOptions> options)
        : base(userManager, roleManager, options)
    {
    }

    protected override async Task<ClaimsIdentity> GenerateClaimsAsync(ApplicationUser user)
    {
        var identity = await base.GenerateClaimsAsync(user);

        identity.AddClaim(new Claim(ClaimsPersonalizados.NombreCompleto, user.NombreCompleto));

        return identity;
    }
}
