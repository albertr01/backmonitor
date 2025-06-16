using WebApplication1.Models.Response.Local.Salida.Juridico;
using WebApplication1.Models.Response.Local.Salida.Natural;

namespace WebApplication1.Models.Response.Local.Salida
{
    public class ConoceTuClienteSalida
    {
        public FichaClienteNaturalSalida fichaClienteNaturalSalida { get; set; }
        public FichaClienteJuridicoSalida fichaClienteJuridicoSalida { get; set; }
        public FichaClienteJuridicoSalida fichaClienteProveedorSalida { get; set; }
        public FichaClienteNaturalSalida fichaClienteEmpleadoSalida { get; set; }
    }
}
