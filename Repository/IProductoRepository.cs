using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public interface IProductoRepository
    {
        PRODUCTO BuscarPorId(int id);

        List<PRODUCTO> Listar();

        (List<PRODUCTO>, int) ListarConPaginacion(PRODUCTO producto, int inicio, int registrosPorPagina);

        PRODUCTO Guardar(PRODUCTO producto);

        PRODUCTO Actualizar(PRODUCTO producto);

        void ActualizarSinSP(PRODUCTO producto);

        void Eliminar(int id);
    }
}
