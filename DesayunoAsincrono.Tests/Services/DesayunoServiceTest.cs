using DesayunoAsincrono.Services;

namespace DesayunoAsincrono.Tests.Services;

public class DesayunoServiceTests
{
    private readonly DesayunoService _service = new();

    [Test]
    public async Task PrepararConTimeoutAsync_SolucionRapida_DevuelveTrue()
    {
        Func<Task> solucionRapida = () => Task.Delay(50);

        var resultado = await _service.PrepararConTimeoutAsync(solucionRapida, 500);

        Assert.That(resultado, Is.True);
    }

    [Test]
    public async Task PrepararConTimeoutAsync_SolucionLenta_DevuelveFalse()
    {
        Func<Task> solucionLenta = () => Task.Delay(1000);

        var resultado = await _service.PrepararConTimeoutAsync(solucionLenta, 200);

        Assert.That(resultado, Is.False);
    }
}