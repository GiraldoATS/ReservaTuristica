using FondoXYZ.Entities.DTOs;

namespace FondoXYZ.Services.Interfaces
{
    public interface IAuthService
    {
        Task<bool> LoginAsync(LoginDto dto);

        Task<(bool Success, string ErrorMessage)>
            RegisterAsync(RegisterDto dto);

        Task LogoutAsync();

        Task ForgotPasswordAsync(string email);

        Task<(bool Success, string ErrorMessage)>
            ResetPasswordAsync(ResetPasswordDto dto);
    }
}