using sgd.be;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sgd.da.Contrato
{
    public interface IAplicacionDa
    {
        AplicacionBe? ObtenerPorCodigo(string piCodigo, SqlConnection piCn);
    }
}
