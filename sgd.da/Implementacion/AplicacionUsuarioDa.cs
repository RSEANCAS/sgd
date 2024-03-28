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
    public class AplicacionUsuarioDa : IAplicacionUsuarioDa
    {

        readonly ILogger<AplicacionUsuarioDa> _logger;
        public AplicacionUsuarioDa(ILogger<AplicacionUsuarioDa> logger)
        {
            _logger = logger;
        }

        public AplicacionUsuarioBe? Obtener(int? piAplicacionId, string? piUsuarioId, SqlConnection piCn)
        {
            AplicacionUsuarioBe? vItem = null;

            try
            {
                using (SqlCommand vCmd = new SqlCommand("dbo.usp_aplicacionusuario_obtener", piCn))
                {
                    vCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    vCmd.Parameters.Add(new SqlParameter { ParameterName = "@piAplicacionId", SqlDbType = System.Data.SqlDbType.Int, Value = piAplicacionId });
                    vCmd.Parameters.Add(new SqlParameter { ParameterName = "@piUsuarioId", SqlDbType = System.Data.SqlDbType.VarChar, Size = 20, Value = piUsuarioId });

                    using (SqlDataReader vDr = vCmd.ExecuteReader())
                    {
                        if (vDr.HasRows)
                        {
                            vItem = new AplicacionUsuarioBe();

                            if (vDr.Read())
                            {
                                vItem.AplicacionId = vDr.GetData<int?>("AplicacionId");
                                vItem.UsuarioId = vDr.GetData<string?>("UsuarioId");
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
