
namespace DesayunoAsincrono.Service;

public interface IDesayunoService
{
    void PrepararSecuencial(IEnumerable<Accion> acciones);
    Task prepararAsincronoAsync(IEnumerable<Accione> acciones);
    Task PrepararMejorRendimiento(IEnumerable<Accion> acciones);
    Task<bool> PrepararConMejorTiempo(Func<Task> solucion, int timeMs);
}

