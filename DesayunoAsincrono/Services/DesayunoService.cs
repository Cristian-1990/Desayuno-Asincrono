using DesayunoAsincrono.Models;

namespace DesayunoAsincrono.Services;
/// <summary>
/// Implementación de la interfaz <see cref="IDesayunoService"/>
/// </summary>
public class DesayunoService : IDesayunoService
{
    /// <summary>
    /// Recorre la lista de acciones una a una 
    /// </summary>
    /// <param name="acciones">Acciones del desayuno</param>
    public void PrepararSecuencial(IEnumerable<Accion> acciones)
    {
        //Recorre toda la lista de acciones una a una bloqueando el hilo hasta que se cumpla la tarea
        foreach (var accion in acciones)
        {
            Console.WriteLine($"Iniciando: {accion.Nombre}");
            Task.Delay(accion.DuracionMs).Wait();
            Console.WriteLine($"Terminado: {accion.Nombre}");
        }
    }
    /// <summary>
    /// Recorre la lista sin bloquear el hilo, pero sigue esperando a que termine una acción para hacer otra.
    /// Como no hay más tareas que requieran ese hilo, el tiempo es prácticamente el mismo eso.
    /// </summary>
    /// <param name="acciones">Acciones del desayuno</param>
    public async Task PrepararAsincronoAsync(IEnumerable<Accion> acciones)
    {
        foreach (var accion in acciones)
        {
            Console.WriteLine($"Iniciando: {accion.Nombre}");
            await Task.Delay(accion.DuracionMs);
            Console.WriteLine($"Terminado: {accion.Nombre}");
        }
    }
    /// <summary>
    /// Ejecuta todas las acciones posibles de forma paralela
    /// </summary>
    /// <param name="acciones"></param>
    public async Task PrepararMejorRendimientoAsync(IEnumerable<Accion> acciones)
    {
        
        //Sacamos todas las acciones de la lista para poder nombrarlas individualmente sin necesidad de recorrer un lista
        var lista = acciones.ToList();
        var cafe = lista.First(a => a.Nombre == "Café");
        var sarten = lista.First(a => a.Nombre == "Sartén");
        var huevos = lista.First(a => a.Nombre == "Huevos");
        var bacon = lista.First(a => a.Nombre == "Bacon");
        var pan = lista.First(a => a.Nombre == "Pan");
        var mantequilla = lista.First(a => a.Nombre == "Mantequilla");
        var zumo = lista.First(a => a.Nombre == "Zumo");
        //Acciones que no necesitan de otras y se pueden ejecutar en paralelo
        var tareaCafe = EjecutarAccionAsync(cafe);
        var tareaZumo = EjecutarAccionAsync(zumo);
        var tareaSarten = EjecutarAccionAsync(sarten);
        var tareaPan = EjecutarAccionAsync(pan);
        //Acciones que si necesitan de otras para realizarse
        var tareaHuevos = EjecutarDespuesDe(tareaSarten, huevos);
        var tareaBacon = EjecutarDespuesDe(tareaSarten, bacon);
        var tareaMantequilla = EjecutarDespuesDe(tareaPan, mantequilla);
        
        await Task.WhenAll(tareaCafe, tareaZumo, tareaSarten, tareaHuevos, tareaBacon, tareaPan, tareaMantequilla);
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="solucion"></param>
    /// <param name="timeoutMs"></param>
    /// <returns></returns>
        public async Task<bool> PrepararConTimeoutAsync(Func<Task> solucion, int timeoutMs)
        {
            var tareaSolucion = solucion();
            var tareaTimeout = Task.Delay(timeoutMs);

            var ganadora = await Task.WhenAny(tareaSolucion, tareaTimeout);

            return ganadora == tareaSolucion;
        }
    
    /// <summary>
    ///  Ejecuta una acción individual: imprime su inicio, espera su duración simulada
    /// </summary>
    /// <param name="accion">La acción a ejecutar</param>
    private static async Task EjecutarAccionAsync(Accion accion)
    {
        Console.WriteLine($"Iniciando: {accion.Nombre}");
        await Task.Delay(accion.DuracionMs);
        Console.WriteLine($"Terminado: {accion.Nombre}");
    }
    /// <summary>
    /// Espera a que termine una tarea de la que depende, y solo entonces ejecuta
    /// la acción indicada.
    /// </summary>
    /// <param name="dependencia">Tarea que debe terminar antes de lanzar la siguiente</param>
    /// <param name="accion">La acción a ejecutar</param>
    private static async Task EjecutarDespuesDe(Task dependencia, Accion accion)
    {
        await dependencia;
        await EjecutarAccionAsync(accion);
    }
}