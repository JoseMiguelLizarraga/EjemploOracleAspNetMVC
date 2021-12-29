using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public interface ICategoriaRepository
    {
        CATEGORIA BuscarPorId(int id);

        List<CATEGORIA> Listar();

        (List<CATEGORIA>, int) ListarConPaginacion(CATEGORIA categoria, int inicio, int registrosPorPagina);

        (IQueryable<CATEGORIA>, int) LlenarSelect2(string busqueda, int registrosPorPagina, int numeroPagina);

        CATEGORIA Guardar(CATEGORIA categoria);

        CATEGORIA Actualizar(CATEGORIA categoria);

        bool Eliminar(int id);

        void InsertarSP(CATEGORIA categoria);
    }
}
