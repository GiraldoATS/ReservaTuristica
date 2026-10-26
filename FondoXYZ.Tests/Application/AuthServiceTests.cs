using FondoXYZ.Application.Services;

namespace FondoXYZ.Tests.Application;

public class AuthServiceTests
{
    private const string UrlRestablecimiento = "https://fondoxyz.test/Account/ResetPassword";

    private readonly FakeIdentityService _identity = new();
    private readonly FakeNotificacionService _notificaciones = new();
    private readonly AuthService _sut;

    public AuthServiceTests()
    {
        _sut = new AuthService(_identity, _notificaciones);
    }

    [Fact]
    public async Task SolicitarRecuperacion_ConCorreoNoRegistrado_NoEnviaNada()
    {
        await _sut.SolicitarRecuperacionPasswordAsync("nadie@fondoxyz.test", UrlRestablecimiento);

        Assert.Empty(_notificaciones.Enviados);
    }

    [Fact]
    public async Task SolicitarRecuperacion_ConCorreoRegistrado_EnviaEnlaceConParametrosCodificados()
    {
        _identity.TokensPorEmail["ana+test@fondoxyz.test"] = "token/con+caracteres=";

        await _sut.SolicitarRecuperacionPasswordAsync("ana+test@fondoxyz.test", UrlRestablecimiento);

        var (email, enlace) = Assert.Single(_notificaciones.Enviados);
        Assert.Equal("ana+test@fondoxyz.test", email);
        Assert.Equal(
            $"{UrlRestablecimiento}?email=ana%2Btest%40fondoxyz.test&token=token%2Fcon%2Bcaracteres%3D",
            enlace);
    }
}
