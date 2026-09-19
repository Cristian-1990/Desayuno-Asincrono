public class DesayunoService : IDesayunoService
{
    public void PrepararSecuencial(IEnumerable<Accion> acciones) { }
    public Task PrepararConAsyncAwaitAsync(IEnumerable<Accion> acciones) { }
    public Task PrepararMejorRendimientoAsync(IEnumerable<Accion> acciones) { }
    public Task<bool> PrepararConTimeoutAsync(Func<Task> solucion, int timeoutMs) { }
    
    public void PrepararSecuencial(IEnumerable<Accion> acciones)
    {
        foreach (var accion in acciones)
        {
            Console.WriteLine($"Iniciando: {accion.Nombre}");
            Task.Delay(accion.DuracionMs).Wait();
            Console.WriteLine($"Terminado: {accion.Nombre}");
        }
    }

    public Task PrepararConAsyncAwaitAsync(IEnumerable<Accion> acciones)
        => throw new NotImplementedException();
    public Task PrepararMejorRendimientoAsync(IEnumerable<Accion> acciones)
        => throw new NotImplementedException();
    public Task<bool> PrepararConTimeoutAsync(Func<Task> solucion, int timeoutMs)
        => throw new NotImplementedException();
    
}