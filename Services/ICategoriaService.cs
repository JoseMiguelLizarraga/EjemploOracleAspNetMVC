using DataAccess;
using Mappings.DTO;
using System.Collections.Generic;

namespace Services
{
    public interface ICategoriaService
    {
        RespuestaService<Select2DTO> LlenarSelect2(string busqueda, int registrosPorPagina, int numeroPagina);
        RespuestaService<CATEGORIA> BuscarPorId(int id);
        RespuestaService<List<CATEGORIA>> Listar();
        RespuestaService<DataTableDTO<CategoriaDTO>> LlenarDataTable(CATEGORIA categoria, int inicio, int registrosPorPagina);
        RespuestaService<CATEGORIA> Guardar(CATEGORIA categoria);
        RespuestaService<CATEGORIA> Actualizar(CATEGORIA categoria);
        RespuestaService<bool> Eliminar(int id);
        RespuestaService<bool> GuardarDatosExcel(List<CATEGORIA> listaCategoria);
    }
}