namespace Consola.Helpers
{
    public class Traduction
    {
        public static string GetStatusName(int status)
        {
            return status switch
            {
                1 => "Pendiente",
                2 => "Aprobado",
                3 => "Rechazado",
                4 => "Observado",
                _ => "Desconocido"
            };
        }
    }
}
