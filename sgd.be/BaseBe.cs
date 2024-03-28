using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sgd.be
{
    public class BaseBe
    {
        public bool FlagActivo { get; set; }
        public string? UsuarioIdModificacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
    }
}
