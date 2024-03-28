using sgd.be;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sgd.bl.Contrato
{
    public interface IUsuarioBl
    {
        UsuarioBe? ObtenerUsuario(string piUsuarioId);
        bool AutenticarUsuario(string? piAplicacionCodigo, string? piUsuarioId, string? piContraseña, out UsuarioBe? poUsuario, out List<string> poListaMensajeError);
    }
}
