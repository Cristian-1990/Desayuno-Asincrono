using DesayunoAsincrono.Models;

namespace DesayunoAsincrono.Services;

public interface IDesayunoService
{
    void PrepararSecuencial(IEnumerable<Accion> acciones);
    Task PrepararAsincronoAsync(IEnumerable<Accion> acciones);
    Task PrepararMejorRendimientoAsync(IEnumerable<Accion> acciones);
    Task<bool> PrepararConTimeoutAsync(Func<Task> solucion, int timeMs);
}

