using FondoXYZ.Entities;
using FondoXYZ.Entities.DTOs;
using FondoXYZ.Services.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace FondoXYZ.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser>
            _userManager;

        private readonly SignInManager<ApplicationUser>
            _signInManager;

        private readonly IEmailService
            _emailService;

        public AuthService(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IEmailService emailService)
        {
            _userManager = userManager;

            _signInManager = signInManager;

            _emailService = emailService;
        }
        // =========================
        // LOGIN
        // =========================
        public async Task<bool> LoginAsync(LoginDto dto)
        {
            // LIMPIAR SESIONES ANTERIORES
            await _signInManager.SignOutAsync();

            var user =
                await _userManager
                    .FindByEmailAsync(dto.Email);

            if (user == null)
                return false;

            var result =
                await _signInManager
                    .PasswordSignInAsync(
                        user.UserName!,
                        dto.Password,
                        dto.RememberMe,
                        lockoutOnFailure: false);

            return result.Succeeded;
        }
        // =========================
        // REGISTER
        // =========================

        public async Task<
            (bool Success, string ErrorMessage)>
            RegisterAsync(RegisterDto dto)
        {
            var existingUser =
                await _userManager
                    .FindByEmailAsync(dto.Email);

            if (existingUser != null)
            {
                return (
                    false,
                    "El correo ya está registrado");
            }

            var user = new ApplicationUser
            {
                UserName = dto.Email,
                Email = dto.Email,
                NombreCompleto = dto.NombreCompleto
            };

            var result =
                await _userManager
                    .CreateAsync(
                        user,
                        dto.Password);

            if (!result.Succeeded)
            {
                var errors =
                    string.Join(
                        ", ",
                        result.Errors
                            .Select(x => x.Description));

                return (false, errors);
            }

            return (true, string.Empty);
        }

        // =========================
        // LOGOUT
        // =========================

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        // =========================
        // FORGOT PASSWORD
        // =========================

        public async Task ForgotPasswordAsync(
            string email)
        {
            var user =
                await _userManager
                    .FindByEmailAsync(email);

            if (user == null)
                return;

            var token =
                await _userManager
                    .GeneratePasswordResetTokenAsync(user);

            token = Uri.EscapeDataString(token);

            var link =
                 $"https://localhost:7155/Account/ResetPassword?email={email}&token={token}";

            var body = $@"
<!DOCTYPE html>
<html lang='es'>
<head>
    <meta charset='UTF-8'>
    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
</head>
<body style='margin:0; padding:40px 0; background-color:#f9f9f9; font-family:-apple-system,BlinkMacSystemFont,""Segoe UI"",Roboto,Helvetica,Arial,sans-serif;'>

    <table width='100%' border='0' cellpadding='0' cellspacing='0' style='max-width:540px; margin:0 auto; background:#ffffff; border-radius:12px; border:1px solid #ededed; overflow:hidden;'>
        
        <!-- CONTENIDO -->
        <tr>
            <td style='padding:48px 40px 32px 40px;'>
                <h1 style='margin:0 0 24px 0; font-size:22px; font-weight:700; color:#111111; letter-spacing:-0.5px;'>
                    FondoXYZ
                </h1>
                
                <p style='margin:0 0 16px 0; font-size:15px; line-height:24px; color:#444444;'>
                    Recibimos una solicitud para restablecer la contraseña de tu cuenta.
                </p>
                
                <p style='margin:0 0 32px 0; font-size:15px; line-height:24px; color:#444444;'>
                    Haz clic en el botón de abajo para configurar una nueva credencial:
                </p>

                <!-- BOTÓN -->
                <table border='0' cellpadding='0' cellspacing='0' style='margin:32px 0;'>
                    <tr>
                        <td align='center' bgcolor='#111111' style='border-radius:6px;'>
                            <a href='{link}' target='_blank' style='display:inline-block; padding:14px 28px; font-size:14px; font-weight:600; color:#ffffff; text-decoration:none;'>
                                Restablecer contraseña
                            </a>
                        </td>
                    </tr>
                </table>

                <p style='margin:32px 0 0 0; font-size:13px; line-height:20px; color:#888888;'>
                    Si no solicitaste este movimiento, puedes ignorar este mensaje con total tranquilidad.
                </p>
            </td>
        </tr>

        <!-- FOOTER -->
        <tr>
            <td style='padding:0 40px 40px 40px; font-size:12px; color:#aaaaaa;'>
                <hr style='border:0; border-top:1px solid #ededed; margin:0 0 20px 0;'>
                © {DateTime.Now.Year} FondoXYZ. Todos los derechos reservados.
            </td>
        </tr>

    </table>

</body>
</html>
";

            await _emailService.SendAsync(
                email,
                "Recuperación de contraseña",
                body);
        }

        // =========================
        // RESET PASSWORD
        // =========================

        public async Task<
    (bool Success, string ErrorMessage)>
    ResetPasswordAsync(
        ResetPasswordDto dto)
        {
            var user =
                await _userManager
                    .FindByEmailAsync(dto.Email);

            if (user == null)
            {
                return (
                    false,
                    "Usuario no encontrado");
            }

            var decodedToken =
                Uri.UnescapeDataString(dto.Token);

            var result =
                await _userManager
                    .ResetPasswordAsync(
                        user,
                        decodedToken,
                        dto.Password);

            if (!result.Succeeded)
            {
                var errors =
                    string.Join(
                        ", ",
                        result.Errors
                            .Select(x => x.Description));

                return (false, errors);
            }

            return (true, string.Empty);
        }
    }
}