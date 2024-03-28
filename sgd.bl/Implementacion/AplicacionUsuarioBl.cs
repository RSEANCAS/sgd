using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using sgd.be;
using sgd.bl.Contrato;
using sgd.da.Contrato;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sgd.bl.Implementacion
{
    public class AplicacionUsuarioBl : IAplicacionUsuarioBl
    {
        readonly ILogger<AplicacionUsuarioBl> _logger;
        readonly string? _cadenaConexion;
        IConfiguration _config;
        IAplicacionUsuarioDa _aplicacionUsuarioDa;

        public AplicacionUsuarioBl(ILogger<AplicacionUsuarioBl> logger, IConfiguration config, IAplicacionUsuarioDa aplicacionUsuarioDa)
        {
            _logger = logger;
            _cadenaConexion = config.GetConnectionString("dbSGD");
            _config = config;
            _aplicacionUsuarioDa = aplicacionUsuarioDa;
        }

        public AplicacionUsuarioBe? ObtenerAplicacionUsuario(int? piAplicacionId, string? piUsuarioId)
        {
            AplicacionUsuarioBe? vItem = null;
            SqlConnection? vCn = null;

            try
            {
                using (vCn = new SqlConnection(_cadenaConexion))
                {
                    vCn.Open();

                    vItem = _aplicacionUsuarioDa.Obtener(piAplicacionId, piUsuarioId, vCn);

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
    }
}
