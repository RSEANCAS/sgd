using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using sgd.be;
using sgd.bl.Contrato;
using sgd.da.Contrato;
using sgd.util;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sgd.bl.Implementacion
{
    public class UsuarioBl : IUsuarioBl
    {
        readonly ILogger<UsuarioBl> _logger;
        readonly string? _cadenaConexion;
        IConfiguration _config;
        IUsuarioDa _usuarioDa;

        IAplicacionBl _aplicacionBl;
        IAplicacionUsuarioBl _aplicacionUsuarioBl;

        public UsuarioBl(ILogger<UsuarioBl> logger, IConfiguration config, IUsuarioDa usuarioDa, IAplicacionBl aplicacionBl, IAplicacionUsuarioBl aplicacionUsuarioBl)
        {
            _logger = logger;
            _cadenaConexion = config.GetConnectionString("dbSGD");
            _config = config;
            _usuarioDa = usuarioDa;
            _aplicacionBl = aplicacionBl;
            _aplicacionUsuarioBl = aplicacionUsuarioBl;
        }

        public UsuarioBe? ObtenerUsuario(string piUsuarioId)
        {
            UsuarioBe? vItem = null;
            SqlConnection? vCn = null;

            try
            {
                using (vCn = new SqlConnection(_cadenaConexion))
                {
                    vCn.Open();

                    vItem = _usuarioDa.Obtener(piUsuarioId, vCn);

                    vCn.Close();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
            finally
            {
                if (vCn != null)
                {
                    if (vCn.State == System.Data.ConnectionState.Open) vCn.Close();
                }
            }

            return vItem;
        }

        public bool AutenticarUsuario(string? piAplicacionCodigo, string? piUsuarioId, string? piContraseña, out UsuarioBe? poUsuario, out List<string> poListaMensajeError)
        {
            poUsuario = null;
            poListaMensajeError = new List<string>();
            bool vEsValido = false;

            if (string.IsNullOrEmpty(piAplicacionCodigo))
            {
                poListaMensajeError.Add($"El código de la aplicación no puede ser vacío o nulo");
                return vEsValido;
            }

            AplicacionBe? vItemAplicacion = _aplicacionBl.ObtenerAplicacionPorCodigo(piAplicacionCodigo);

            if (vItemAplicacion == null)
            {
                poListaMensajeError.Add($"No se encontró ninguna aplicación con el código {piAplicacionCodigo}");
                return vEsValido;
            }

            if (string.IsNullOrEmpty(piUsuarioId))
            {
                poListaMensajeError.Add($"El Id del usuario no puede ser vacío o nulo");
                return vEsValido;
            }

            UsuarioBe? vItemUsuario = ObtenerUsuario(piUsuarioId);

            if (vItemUsuario == null)
            {
                poListaMensajeError.Add($"No se encontró ningún usuario con el código {piUsuarioId}");
                return vEsValido;
            }

            if (string.IsNullOrEmpty(piContraseña))
            {
                poListaMensajeError.Add($"La contraseña no puede ser vacío o nulo");
                return vEsValido;
            }

            byte[] vContraseñaBytes = Seguridad.MD5Byte(piContraseña);
            string vContraseñaBytesStr = Convert.ToBase64String(vContraseñaBytes);
            string vUsuarioContraseñaBytesStr = Convert.ToBase64String(vItemUsuario.Contraseña);

            if (vContraseñaBytesStr != vUsuarioContraseñaBytesStr)
            {
                poListaMensajeError.Add($"La contraseña es incorrecta");
                return vEsValido;
            }

            AplicacionUsuarioBe? vItemAplicacionUsuario = _aplicacionUsuarioBl.ObtenerAplicacionUsuario(vItemAplicacion.AplicacionId, vItemUsuario.UsuarioId);

            if(vItemAplicacionUsuario == null)
            {
                poListaMensajeError.Add($"El usuario {piUsuarioId} no tiene acceso a la aplicación {vItemAplicacion.Nombre}");
                return vEsValido;
            }

            poUsuario = vItemUsuario;

            vEsValido = poListaMensajeError.Count == 0;

            return vEsValido;
        }
    }
}
