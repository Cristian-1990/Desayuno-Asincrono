using DesayunoAsincrono.Config;
using DesayunoAsincrono.Models;
using DesayunoAsincrono.Services;
using System.Diagnostics;
using Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection();
services.AddSingleton<IDesayunoService, DesayunoService>();
var provider = services.BuildServiceProvider();
var desayunoService = provider.GetRequiredService<IDesayunoService>();

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

TimeSpan Medir(Action accion)
{
    var sw = Stopwatch.StartNew();
    accion();
    sw.Stop();
    return sw.Elapsed;
}

async Task<TimeSpan> MedirAsync(Func<Task> accion)
{
    var sw = Stopwatch.StartNew();
    await accion();
    sw.Stop();
    return sw.Elapsed;
}

var tiempoSecuencial = Medir(() => desayunoService.PrepararSecuencial(acciones));
var tiempoAsincrono = await MedirAsync(() => desayunoService.PrepararAsincronoAsync(acciones));
var tiempoMejorRendimiento = await MedirAsync(() => desayunoService.PrepararMejorRendimientoAsync(acciones));

Console.WriteLine($"Secuencial: {tiempoSecuencial.TotalMilliseconds}ms");
Console.WriteLine($"Async/Await: {tiempoAsincrono.TotalMilliseconds}ms");
Console.WriteLine($"Mejor rendimiento: {tiempoMejorRendimiento.TotalMilliseconds}ms");

var okSecuencialTimeout = await desayunoService.PrepararConTimeoutAsync(
    () => Task.Run(() => desayunoService.PrepararSecuencial(acciones)), AppConfig.TimeoutMs);
var okAsincronoTimeout = await desayunoService.PrepararConTimeoutAsync(
    () => desayunoService.PrepararAsincronoAsync(acciones), AppConfig.TimeoutMs);
var okMejorRendimientoTimeout = await desayunoService.PrepararConTimeoutAsync(
    () => desayunoService.PrepararMejorRendimientoAsync(acciones), AppConfig.TimeoutMs);

Console.WriteLine($"Secuencial con timeout: {(okSecuencialTimeout ? "a tiempo" : "cortado")}");
Console.WriteLine($"Async con timeout: {(okAsincronoTimeout ? "a tiempo" : "cortado")}");
Console.WriteLine($"Mejor rendimiento con timeout: {(okMejorRendimientoTimeout ? "a tiempo" : "cortado")}");