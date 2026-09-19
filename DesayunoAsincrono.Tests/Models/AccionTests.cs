using DesayunoAsincrono.Models;

namespace DesayunoAsincrono.Tests.Models;

public class AccionTests
{
    [Test]
    public void DosAccionesConMismosValores_SonIguales()
    {
        var a1 = new Accion("Café", 200);
        var a2 = new Accion("Café", 200);

        Assert.That(a1, Is.EqualTo(a2));
    }
}