using DesayunoAsincrono.Services;
using System.Diagnostics;
using DesayunoAsincrono.Models;

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
    [Test]
    public async Task PrepararMejorRendimientoAsync_TiempoTotal_EsCaminoCriticoNoSuma()
    {
        var acciones = new List<Accion>
        {
            new Accion("Café", 200),
            new Accion("Sartén", 200),
            new Accion("Huevos", 300),
            new Accion("Bacon", 300),
            new Accion("Pan", 200),
            new Accion("Mantequilla", 100),
            new Accion("Zumo", 200),
        };

        var sw = Stopwatch.StartNew();
        await _service.PrepararMejorRendimientoAsync(acciones);
        sw.Stop();

        // Camino crítico teórico: Sartén (200) + max(Huevos, Bacon) (300) = 500ms
        // Si fuera secuencial: 1500ms. Comprobamos que se acerca al camino crítico, no a la suma.
        Assert.That(sw.Elapsed.TotalMilliseconds, Is.LessThan(900));
    }
    
}