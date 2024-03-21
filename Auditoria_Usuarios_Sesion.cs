using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos
{
    public class Auditoria_Usuarios_Sesion
    {
        public int IDAuditoria { get; set; }
        public int UsuarioID { get; set; }
        public DateTime FechaHoraInicio { get; set; }
        public DateTime FechaHoraFin { get; set; }
        public bool Exito { get; set; }
        public string Mensaje { get; set; }
    }
}
