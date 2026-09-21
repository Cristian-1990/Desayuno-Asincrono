using DesayunoAsincrono.Models;

namespace DesayunoAsincrono.Services;
/// <summary>
/// Define las tres versiones del desayuno
/// </summary>
public interface IDesayunoService
{
    /// <summary>
    /// Prepara el desayuno de forma síncrona, bloquea el hilo de ejecución
    /// </summary>
    /// <param name="acciones">Acciones del desayuno que se prepara</param>
    void PrepararSecuencial(IEnumerable<Accion> acciones);
    /// <summary>
    ///Prepara el desayuno sin bloquear el hilo de ejecución, pero las acciones se siguen ejecutando una detras de otra
    /// y por eso tarda practicamente lo mismo que la sincrona.
    /// </summary>
    /// <param name="acciones">Acciones del desayuno que se prepara</param>
    Task PrepararAsincronoAsync(IEnumerable<Accion> acciones);
    /// <summary>
    /// Prepara el desayuno ejecutando todas las acciones que no dependen entre sí.
    /// Encadenando las que si dependen. Tostada -> Untar tostada
    /// </summary>
    /// <param name="acciones">Las acciones del desayuno a preparar</param>
    Task PrepararMejorRendimientoAsync(IEnumerable<Accion> acciones);
    /// <summary>
    /// Ejecuta una de las 3 soluciones posibles con tiempo
    /// Si el límite se cumple primero la solución se abandona aunque sigue corriendo de fondo
    /// </summary>
    /// <param name="solucion">Solucion de las 3 posibles a ejecutar</param>
    /// <param name="timeMs">Límite de tiempo en milisegundos</param>
    /// <returns>Dice si la solución termino dentro del límite de tiempo o no</returns>
    Task<bool> PrepararConTimeoutAsync(Func<Task> solucion, int timeMs);
}

