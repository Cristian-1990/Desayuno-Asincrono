namespace DesayunoAsincrono.Models;
/// <summary>
/// Representa una acción del desayuno.
/// Con nombre y duración.
/// </summary>
/// <param name="Nombre"></param>
/// <param name="DuracionMs"></param>
public record Accion(string Nombre, int DuracionMs);
