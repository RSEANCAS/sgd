using sgd.be;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sgd.bl.Contrato
{
    public interface IAplicacionUsuarioBl
    {
        AplicacionUsuarioBe? ObtenerAplicacionUsuario(int? piAplicacionId, string? piUsuarioId);
    }
}
