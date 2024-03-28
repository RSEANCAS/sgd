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
    public class UsuarioDa : IUsuarioDa
    {
        readonly ILogger<UsuarioDa> _logger;
        public UsuarioDa(ILogger<UsuarioDa> logger)
        {
            _logger = logger;
        }

        public UsuarioBe? Obtener(string piUsuarioId, SqlConnection piCn)
        {
            UsuarioBe? vItem = null;

            try
            {
                using (SqlCommand vCmd = new SqlCommand("dbo.usp_usuario_obtener", piCn))
                {
                    vCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    vCmd.Parameters.Add(new SqlParameter { ParameterName = "@piUsuarioId", SqlDbType = System.Data.SqlDbType.VarChar, Size = 20, Value = piUsuarioId });

                    using (SqlDataReader vDr = vCmd.ExecuteReader())
                    {
                        if (vDr.HasRows)
                        {
                            vItem = new UsuarioBe();

                            if (vDr.Read())
                            {
                                vItem.UsuarioId = vDr.GetData<string?>("UsuarioId");
                                vItem.Contraseña = vDr.GetData<byte[]?>("Contraseña");
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
