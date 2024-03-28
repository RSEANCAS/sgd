using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using sgd.be;
using sgd.bl.Contrato;
using sgd.da;
using sgd.da.Contrato;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sgd.bl.Implementacion
{
    public class AplicacionBl : IAplicacionBl
    {
        readonly ILogger<AplicacionBl> _logger;
        readonly string? _cadenaConexion;
        IConfiguration _config;
        IAplicacionDa _aplicacionDa;

        public AplicacionBl(ILogger<AplicacionBl> logger, IConfiguration config, IAplicacionDa aplicacionDa)
        {
            _logger = logger;
            _cadenaConexion = config.GetConnectionString("dbSGD");
            _config = config;
            _aplicacionDa = aplicacionDa;
        }

        public AplicacionBe? ObtenerAplicacionPorCodigo(string piCodigo)
        {
            AplicacionBe? vItem = null;
            SqlConnection? vCn = null;

            try
            {
                using (vCn = new SqlConnection(_cadenaConexion))
                {
                    vCn.Open();

                    vItem = _aplicacionDa.ObtenerPorCodigo(piCodigo, vCn);

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
