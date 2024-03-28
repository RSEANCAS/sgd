using Microsoft.Extensions.Logging;
using sgd.be;
using sgd.da.Contrato;
using sgd.util;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sgd.da.Implementacion
{
    public class AplicacionDa : IAplicacionDa
    {
        readonly ILogger<AplicacionDa> _logger;
        public AplicacionDa(ILogger<AplicacionDa> logger)
        {
            _logger = logger;
        }

        public AplicacionBe? ObtenerPorCodigo(string piCodigo, SqlConnection piCn)
        {
            AplicacionBe? vItem = null;

            try
            {
                using (SqlCommand vCmd = new SqlCommand("dbo.usp_aplicacion_obtener_por_codigo", piCn))
                {
                    vCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    vCmd.Parameters.Add(new SqlParameter { ParameterName = "@piCodigo", SqlDbType = System.Data.SqlDbType.VarChar, Size = 50, Value = piCodigo });

                    using (SqlDataReader vDr = vCmd.ExecuteReader())
                    {
                        if (vDr.HasRows)
                        {
                            vItem = new AplicacionBe();

                            if (vDr.Read())
                            {
                                vItem.AplicacionId = vDr.GetData<int?>("AplicacionId");
                                vItem.Codigo = vDr.GetData<string?>("Codigo");
                                vItem.Nombre = vDr.GetData<string?>("Nombre");
                                vItem.FlagActivo = vDr.GetData<bool>("FlagActivo");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            return vItem;
        }
    }
}
