using DesayunoAsincrono.Models;

namespace DesayunoAsincrono.Services;

public class DesayunoService : IDesayunoService
{
    public void PrepararSecuencial(IEnumerable<Accion> acciones)
    {
        foreach (var accion in acciones)
        {
            Console.WriteLine($"Iniciando: {accion.Nombre}");
            Task.Delay(accion.DuracionMs).Wait();
            Console.WriteLine($"Terminado: {accion.Nombre}");
        }
    }

    public async Task PrepararAsincronoAsync(IEnumerable<Accion> acciones)
    {
        foreach (var accion in acciones)
        {
            await EjecutarAccionAsync(accion);
        }
    }

    public async Task PrepararMejorRendimientoAsync(IEnumerable<Accion> acciones)
    {
        var lista = acciones.ToList();
        var cafe = lista.First(a => a.Nombre == "Café");
        var sarten = lista.First(a => a.Nombre == "Sartén");
        var huevos = lista.First(a => a.Nombre == "Huevos");
        var bacon = lista.First(a => a.Nombre == "Bacon");
        var pan = lista.First(a => a.Nombre == "Pan");
        var mantequilla = lista.First(a => a.Nombre == "Mantequilla");
        var zumo = lista.First(a => a.Nombre == "Zumo");

        var tareaCafe = EjecutarAccionAsync(cafe);
        var tareaZumo = EjecutarAccionAsync(zumo);

        var tareaSarten = EjecutarAccionAsync(sarten);
        var tareaHuevos = EjecutarDespuesDe(tareaSarten, huevos);
        var tareaBacon = EjecutarDespuesDe(tareaSarten, bacon);

        var tareaPan = EjecutarAccionAsync(pan);
        var tareaMantequilla = EjecutarDespuesDe(tareaPan, mantequilla);

        await Task.WhenAll(tareaCafe, tareaZumo, tareaSarten, tareaHuevos, tareaBacon, tareaPan, tareaMantequilla);
    }

    public Task<bool> PrepararConTimeoutAsync(Func<Task> solucion, int timeoutMs)
        => throw new NotImplementedException();

    private static async Task EjecutarAccionAsync(Accion accion)
    {
        Console.WriteLine($"Iniciando: {accion.Nombre}");
        await Task.Delay(accion.DuracionMs);
        Console.WriteLine($"Terminado: {accion.Nombre}");
    }

    private static async Task EjecutarDespuesDe(Task dependencia, Accion accion)
    {
        await dependencia;
        await EjecutarAccionAsync(accion);
    }
}